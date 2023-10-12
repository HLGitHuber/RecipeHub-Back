using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories
{
    public interface IRecipeRepository
    {
        Task<IEnumerable<Recipe>> GetRecipes(); //+async w nazwie
        Task<Recipe?> GetRecipe(int id);
        Task AddRecipe(Recipe recipe);
        Task<bool> UpdateRecipe(Recipe recipe);
        Task<bool> DeleteRecipe(int recipeid);
        Task<IEnumerable<Recipe>> GetRecipesByIngredientIDs(List<int> ingredientIDs);
        
    }
}
