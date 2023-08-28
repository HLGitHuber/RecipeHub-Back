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

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<int>>> GetAllIngredientIdsByRecipeId(int id)
        {
            var ingredientIds = await _context.RecipeIngredients
                .Where(i => i.RecipeId == id)
                .Select(i => i.IngredientId)
                .ToListAsync();

            if (ingredientIds.Count == 0)
            {
                return NotFound();
            }

            return Ok(ingredientIds);
        }

        [HttpGet("ingredientnames/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<string>>> GetIngredientNamesForRecipeId(int id)
        {
            var ingredientNames = await _context.RecipeIngredients
                .Where(i => i.RecipeId == id)
                .Select(i => i.Ingredient.Name)
                .ToListAsync();

            if (ingredientNames.Count == 0)
            {
                return NotFound();
            }

            return Ok(ingredientNames);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddIngredientsToRecipe([FromQuery] RecipeIngredientForAddDto recipeIngredientForAddDTO)
        {
            if (recipeIngredientForAddDTO == null)
            {
                return BadRequest("RecipeIngredient data is null.");
            }

            var recipe = await _context.Recipes.FindAsync(recipeIngredientForAddDTO.RecipeId);
            var ingredient = await _context.Ingredients.FindAsync(recipeIngredientForAddDTO.IngredientId);

            if (recipe == null || ingredient == null)
            {
                return NotFound("Recipe or Ingredient not found.");
            }

            var newRecipeIngredient = new RecipeIngredient
            {
                Recipe = recipe,
                Ingredient = ingredient
            };

            await _context.RecipeIngredients.AddAsync(newRecipeIngredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("deleteallingredients/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RecipeIngredientDTO>>> DeleteAllIngredientsForRecipe(int recipeId)
        {
            var recipeIngredients = await _context.RecipeIngredients
            .Where(ri => ri.RecipeId == recipeId)
            .ToListAsync();

            if (recipeIngredients.Count == 0)
            {
                return NotFound();
            }
            _context.RecipeIngredients.RemoveRange(recipeIngredients);
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("deletesingleingredient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteSingleIngredientFromRecipe([FromQuery] int recipeid, int ingredientid)
        {
            var recipeIngredient = await _context.RecipeIngredients
                .FirstOrDefaultAsync(ri => ri.RecipeId == recipeid && ri.IngredientId == ingredientid);

            if (recipeIngredient == null)
            {
                return NotFound();
            }

            _context.RecipeIngredients.Remove(recipeIngredient);
            await _context.SaveChangesAsync();

            return NoContent();
        }


    }
}
