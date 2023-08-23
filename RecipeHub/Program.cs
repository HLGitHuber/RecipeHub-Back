using System.Reflection;
using RecipeHub.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Infrastructure.Repositories;


namespace RecipeHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
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