using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;

namespace RecipeHub.Infrastructure
{
    public class RecipeDBContext : DbContext
    {
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Recipe> Recipes => Set<Recipe>();
        public DbSet<User> Users =>Set <User>();
        public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
        public RecipeDBContext(DbContextOptions<RecipeDBContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecipeIngredient>()
                .HasKey(ri => new {ri.RecipeId, ri.IngredientId});
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Recipe)
                .WithMany(ri => ri.Ingredients)
                .HasForeignKey(ri => ri.RecipeId);
            modelBuilder.Entity<RecipeIngredient>()
                .HasOne(ri => ri.Ingredient)
                .WithMany(ri => ri.Recipes)
                .HasForeignKey(ri => ri.IngredientId);

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
