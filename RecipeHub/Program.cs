using System.Reflection;
using RecipeHub.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Infrastructure.Repositories;
using System.Net;
using System.Text.Json;
using Serilog;
using RecipeHub.Configuration.Extensions;
using Microsoft.AspNetCore.Identity;
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
            
            //builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
            
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
            builder.Services.AddSwaggerGen();

            builder.Services.AddResponseCaching();
            builder.Services.AddMemoryCache();

            builder.Services.AddProblemDetails();

            builder.Host.UseSerilog((context, services, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
                configuration.ReadFrom.Services(services);

            }, preserveStaticLogger: true);

            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 12;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;

                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789;:,<.>/?-_=+!@#$%^&*()";
                options.User.RequireUniqueEmail = true;

            })
                .AddEntityFrameworkStores<RecipeDBContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication()
                .AddCookie(options =>
                {
                    options.Cookie.Name = "RecipeHub.Cookies";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SameSite = SameSiteMode.Strict;

                    options.Events.OnRedirectToLogin = context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;

                    };
                    options.LoginPath = "/api/users/login-cookie";
                    options.LogoutPath = "/api/users/logout-cookie";
                    options.AccessDeniedPath = "/api/users/access-denied";
                });
            builder.Services.AddAuthentication();

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

            app.UseCors();

            app.UseResponseCaching();
            
            app.MapControllers();

            app.Run();
        }

    }
}