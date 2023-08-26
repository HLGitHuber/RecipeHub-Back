using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;

namespace RecipeHub.Infrastructure
{
    public class RecipeDBContext : IdentityDbContext<User, Role, string>
    {
        public DbSet<Ingredient> Ingredients => Set<Ingredient>();
        public DbSet<Recipe> Recipes => Set<Recipe>();
        //public DbSet<User> Users =>Set <User>(); not necessary if user does not have extra properties
        public override DbSet<Role> Roles => Set<Role>();
        public DbSet<RecipeIngredient> RecipeIngredients => Set<RecipeIngredient>();
        public RecipeDBContext(DbContextOptions<RecipeDBContext> options):base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            modelBuilder.Entity<Role>()
                .HasData(new List<Role>
                {
                    new Role
                    {
                        Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN"
                    },
                    new Role
                    {
                        Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER"
                    }

            });


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

            //modelBuilder.Entity<Recipe>()
            //    .HasData(new List<Recipe>()
            //    {
            //        new Recipe()
            //        {
            //            Id = 2,
            //            Calories = 1000,
            //            Name = "Milk with cheese",
            //            PreparationTimeMin = 1,
            //            PreparationTimeMax = 2,
            //            IngredientsText = "IngredientsText",
            //            RecipeText = "RecipeText",
            //            UserId = 0
            //        }
            //    });

            modelBuilder.Entity<RecipeIngredient>()
                .HasData(new List<RecipeIngredient>()
                {
                    new RecipeIngredient()
                    {
                        IngredientId = 1, // Milk
                        RecipeId = 2
                    },
                    new RecipeIngredient()
                    {
                        IngredientId = 3, // Cheese
                        RecipeId = 2
                    }
                });


        }
    }
}
