using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Controllers
{
    [ApiController]
    [Route("api/recipeingredient")]
    public class RecipeIngredientController: ControllerBase
    {
        private readonly RecipeDBContext _context;
        public RecipeIngredientController(RecipeDBContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddIngredientsToRecipe(RecipeIngredientDTO recipeIngredientDTO)
        {
            if (recipeIngredientDTO == null)
            {
                return BadRequest("RecipeIngredient data is null.");
            }

            var recipe = _context.Recipes.Find(recipeIngredientDTO.RecipeId);
            var ingredient = _context.Ingredients.Find(recipeIngredientDTO.IngredientId);

            if (recipe == null || ingredient == null)
            {
                return NotFound("Recipe or Ingredient not found.");
            }

            var newRecipeIngredient = new RecipeIngredient
            {
                Recipe = recipe,
                Ingredient = ingredient
            };

            _context.RecipeIngredients.Add(newRecipeIngredient);
            _context.SaveChanges();

            return CreatedAtAction(nameof(AddIngredientsToRecipe), new { id = newRecipeIngredient.RecipeId }, newRecipeIngredient);
        }
    }
}
