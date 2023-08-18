using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories
{
    public interface IRecipeRepository
    {
        IEnumerable<Recipe> GetRecipe(string? search);
        Recipe? GetRecipe(int id);
        void AddRecipe(Recipe recipe);
        bool UpdateRecipe(Recipe recipe);
        bool DeleteRecipe(int id);
    }
}
