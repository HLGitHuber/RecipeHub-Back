using System.Reflection;
using RecipeHub.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Infrastructure.Repositories;
using System.Net;
using System.Text.Json;
using Serilog;

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

            builder.Services.AddDbContext<RecipeDBContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("PGSQLDb"));
                options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
            });
            
            builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddConsole());
            
            builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();

            builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
            
            builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
            
            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(policyBuilder =>
                {
                    policyBuilder
                        .WithOrigins("http://localhost:3000", "http://localhost:5173")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });
            
            
            
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