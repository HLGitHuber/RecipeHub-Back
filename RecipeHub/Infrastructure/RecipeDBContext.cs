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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ingredient>()
                .HasData(new List<Ingredient>()
                {
                    new Ingredient()
                    {
                        Id = 1, Name = "Milk"
                    },
                    new Ingredient()
                    {
                        Id = 2, Name = "Butter"
                    },
                    new Ingredient()
                    {
                        Id = 3, Name = "Cheese"
                    }
                });
        }
    }
}
