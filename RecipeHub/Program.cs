using System.Reflection;
using RecipeHub.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Infrastructure.Repositories;
using System.Net;

using System.Security.Claims;
using System.Text;

using System.Text.Json;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using RecipeHub.Configuration.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RecipeHub.Configuration.Options;
using RecipeHub.Domain;

namespace RecipeHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger(); 

            builder.AddPersistence();
            
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
            
            builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
            
            builder.Services.AddScoped<IRecipeIngredientRepository, RecipeIngredientRepository>();

            builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();

            
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

            builder.AddCors();



            // Add services to the container.

            builder.Services.AddControllers(configure =>
            {
                configure.CacheProfiles.Add("Any-60",
                    new CacheProfile()
                    {
                        Location = ResponseCacheLocation.Any,
                        Duration = 60
                    });
                configure.CacheProfiles.Add("NoCache", new CacheProfile() {NoStore = true});
            }).AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "RecipeHub.Api",
                        Version = "v1"
                    });
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Here Enter JWT Token with bearer format like bearer[space] token"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                    new OpenApiSecurityScheme()
                    {
                        Name = "Bearer",
                        In = ParameterLocation.Header,
                        Reference = new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                    }
                });
                });
            
            

            builder.Services.AddResponseCaching();
            builder.Services.AddMemoryCache();

            builder.Services.AddProblemDetails();

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
                configuration.ReadFrom.Services(services);

            }, preserveStaticLogger: true);

            builder.Services.AddIdentity<User, Role>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789;:,<.>/?-_=+!@#$%^&*()";
                options.User.RequireUniqueEmail = true;

            })
                .AddEntityFrameworkStores<RecipeDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddOptions<JwtConfiguration>()
                .Bind(builder.Configuration.GetSection(JwtConfiguration.SectionName))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["Authentication:Jwt:Issuer"],

                        ValidateAudience = true,
                        ValidAudience = builder.Configuration["Authentication:Jwt:Audience"],

                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Jwt:SigningKey"]!)),

                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });
            builder.Services.AddAuthorization(options =>
            {
                var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                    JwtBearerDefaults.AuthenticationScheme);
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();

                options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
                
                options.AddPolicy("AdminOnly", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim(ClaimTypes.Role, "Admin");
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else if (app.Environment.IsProduction())
            {
                app.UseExceptionHandler(applicationBuilder =>
                {
                    applicationBuilder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        context.Response.ContentType = "application/json";


                        var problemDetailsObject = new ProblemDetails
                        {
                            Title = "Unexpected problem occured",
                            Status = context.Response.StatusCode,
                            Detail = "We have received information about your issue, we will work on it. Please try again later."
                        };

                        var problemDetailsJson = JsonSerializer.Serialize(problemDetailsObject);

                        await context.Response.WriteAsync(problemDetailsJson);
                    });
                });

                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.UseAuthentication();

            app.UseCors();

            app.UseResponseCaching();
            
            app.MapControllers();

            app.Run();
        }

    }
}