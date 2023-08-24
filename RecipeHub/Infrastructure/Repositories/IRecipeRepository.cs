﻿using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetRecipes();
        Recipe? GetRecipe(int id);
        void AddRecipe(Recipe recipe);
        bool UpdateRecipe(Recipe recipe);
        bool DeleteRecipe(int id);
        public Task<IEnumerable<Recipe>> GetRecipesByIngredientIDs(List<int> ingredientIDs);
        IEnumerable<Recipe> GetRecipesByIngredients(List<int>? ingredientIds);
    }
}
