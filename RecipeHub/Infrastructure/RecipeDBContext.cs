using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;

namespace RecipeHub.Infrastructure
{
    public class RecipeDBContext:DbContext
    {
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<User> Users => Set<User>();
        public RecipeDBContext(DbContextOptions<RecipeDBContext> options):base(options) { }
    }
}
