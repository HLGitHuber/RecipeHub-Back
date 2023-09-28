using System;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;
using Serilog;

namespace RecipeHub.Infrastructure.Repositories
{
	public class UserFavouriteRecipesRepository : IUserFavouriteRecipesRepository
	{
        private readonly RecipeDBContext _dbContext;

        public UserFavouriteRecipesRepository(RecipeDBContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Recipe>> GetUserFavouriteRecipesAsync(string userId)
        {
            var user = await _dbContext.Users
                .Include(u => u.FavouriteRecipes)
                .ThenInclude(uf => uf.Recipe)
                .FirstOrDefaultAsync(u => u.Id == userId);

            return user?.FavouriteRecipes.Select(uf => uf.Recipe).ToList() ?? new List<Recipe>();
        }

        public async Task<bool> AddRecipeToUserFavouritesAsync(string userId, int recipeId)
        {
            var user = await _dbContext.Users.FindAsync(userId);
            var recipe = await _dbContext.Recipes.FindAsync(recipeId);

            if (user != null && recipe != null)
            {
                if (!user.FavouriteRecipes.Any(uf => uf.RecipeId == recipeId))
                {
                    user.FavouriteRecipes.Add(new UserFavouriteRecipes { UserId = userId, RecipeId = recipeId });
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

        public async Task<bool> DeleteRecipeFromUserFavouritesAsync(string userId, int recipeId)
        {
            var user = await _dbContext.Users
                .Include(u => u.FavouriteRecipes)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user != null)
            {
                var userFavourite = user.FavouriteRecipes.FirstOrDefault(uf => uf.RecipeId == recipeId);

                if (userFavourite != null)
                {
                    user.FavouriteRecipes.Remove(userFavourite);
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }

    }
}