using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;
using RecipeHub.Infrastructure;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Configuration.Extensions
{
    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddPersistence(this WebApplicationBuilder builder)
        {

            builder.Services.AddDbContext<RecipeDBContext>(options =>
            {
                options.UseNpgsql(builder.Configuration.GetConnectionString("PGSQLDb"));
                options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
            });

            builder.Services.AddScoped<IIngredientsRepository, IngredientsRepository>();

            //builder.Services.AddIdentity<User, Role>()
            //    .AddEntityFrameworkStores<RecipeDBContext>();
            //builder.Services.AddIdentityCore<Role>() // This is used to configure RoleManager
            //    .AddRoleManager<RoleManager<Role>>();

            return builder;
        }
        public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
        {
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
            return builder;

        }
    }


}
