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
        public DbSet<UserFavouriteRecipe> UserFavouriteRecipes => Set<UserFavouriteRecipe>();

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

            modelBuilder.Entity<UserFavouriteRecipe>()
                .HasKey(uf => new { uf.UserId, uf.RecipeId });
            modelBuilder.Entity<UserFavouriteRecipe>()
                .HasOne(uf => uf.User)
                .WithMany(u => u.FavouriteRecipes)
                .HasForeignKey(uf => uf.UserId);
            modelBuilder.Entity<UserFavouriteRecipe>()
                .HasOne(uf => uf.Recipe)
                .WithMany(r => r.UsersFavoritedBy)
                .HasForeignKey(uf => uf.RecipeId);
        }
    }
}
