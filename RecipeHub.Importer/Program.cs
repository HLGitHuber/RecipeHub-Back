using Newtonsoft.Json;
using System.Net.Http.Headers;
using RecipeHub.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RecipeHub.Domain;

namespace RecipeHub.Importer
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            string connectionString = configuration.GetConnectionString("PGSQLDb");

            var serviceProvider = new ServiceCollection()
                .AddDbContext<RecipeDBContext>(options => options.UseNpgsql(connectionString))
                .BuildServiceProvider();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Add("User-Agent", ".NET Foundation Repository Reporter");

            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RecipeDBContext>();
                await ProcessIngredientsAsync(new HttpClient(), dbContext);
            }

        }

        static async Task ProcessIngredientsAsync(HttpClient client, RecipeDBContext dbContext)
        {
            var jsonResponse = await client.GetStringAsync(
                "https://www.themealdb.com/api/json/v1/1/list.php?i=list");

            List<IngredientData> ingredientDataResponse = JsonConvert.DeserializeObject<IngredientDataResponse>(jsonResponse).Meals;

            foreach (var ingredientData in ingredientDataResponse)
            {
                var existingIngredient =
                    await dbContext.Ingredients.FirstOrDefaultAsync(i => i.Name == ingredientData.strIngredient);

                if (existingIngredient != null)
                {
                    continue;
                }

                var ingredient = new Ingredient()
                {
                    Name = ingredientData.strIngredient
                };

                dbContext.Ingredients.Add(ingredient);
            }

            await dbContext.SaveChangesAsync();
        }

    }
}