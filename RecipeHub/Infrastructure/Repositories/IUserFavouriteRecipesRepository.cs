using System;
using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories
{
    public interface IUserFavouriteRecipesRepository
    {
        Task<IEnumerable<Recipe>> GetUserFavouriteRecipesAsync(string userId);
        Task<bool> AddRecipeToUserFavouritesAsync(string userId, int recipeId);
        Task<bool> DeleteRecipeFromUserFavouritesAsync(string userId, int recipeId);
    }
}

