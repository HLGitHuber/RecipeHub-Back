using Microsoft.AspNetCore.Mvc;
using RecipeHub.Domain;
using RecipeHub.DTO_s;

namespace RecipeHub.Infrastructure.Repositories
{
    public interface IRecipeIngredientRepository
    {
        Task<ActionResult<IEnumerable<int>>> GetAllIngredientIdsByRecipeId(int id);
        Task<ActionResult<List<string>>> GetIngredientNamesForRecipeId(int id);
        Task<RecipeIngredient> AddIngredientsToRecipe([FromQuery] RecipeIngredientForAddDto recipeIngredientForAddDTO);
        Task<bool> DeleteAllIngredientsForRecipe(int id);
        Task<bool> DeleteSingleIngredientFromRecipe([FromQuery] int recipeid, int ingredientid)


    }
}
