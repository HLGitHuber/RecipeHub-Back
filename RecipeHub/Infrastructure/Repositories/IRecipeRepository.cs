using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetRecipes();
        Task<Recipe?> GetRecipe(int id);
        void AddRecipe(Recipe recipe);
        bool UpdateRecipe(Recipe recipe);
        bool DeleteRecipe(int id);
        Task<IEnumerable<Recipe>> GetRecipesByIngredientIDs(List<int> ingredientIDs);
        
    }
}
