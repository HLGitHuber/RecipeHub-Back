using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Controllers
{
    [Route("api/Recipe")]
    [ApiController]
    public class RecipeController: ControllerBase
    {
        private readonly RecipeDBContext _context;
        private readonly IIngredientsRepository _repository;
        private readonly IMapper _mapper;

        public RecipeController(RecipeDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<RecipesAllDto>> GetAllRecipes()
        {
            return Ok(_context.Recipes);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<RecipeDTO>> GetRecipeByID(int id)
        {
            var recipe = _context.Recipes.Include(i=>i.Ingredients).FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NoContent();
            }
            var recipeDto = new RecipeDTO()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                PreparationTimeMax = recipe.PreparationTimeMax,
                PreparationTimeMin = recipe.PreparationTimeMin,
                IngredientsText = recipe.IngredientsText,
                RecipeText = recipe.RecipeText,
                Calories = recipe.Calories,              
            };
            return Ok(recipeDto);
        }

        [HttpGet("recipesbyingredients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Recipe>>> GetRecipesByIngredientIDs([FromQuery] List<int> ingredientIDs)
        {
            if (ingredientIDs == null || ingredientIDs.Count == 0)
            {
                return BadRequest("No ingredient IDs provided.");
            }

            var recipes = await _context.Recipes
                .Where(recipe => recipe.Ingredients.All(ri => ingredientIDs.Contains(ri.IngredientId)))
                .ToListAsync();

            return Ok(recipes);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddRecipe([FromBody] RecipeForAddDto recipeForAddDto)
        {
            if (string.IsNullOrWhiteSpace(recipeForAddDto.Name))
            {
                ModelState.AddModelError("emptyName", "Name cannot be empty!");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var recipe = _mapper.Map<Recipe>(recipeForAddDto);

            _context.Recipes.Add(recipe);
            _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAllRecipes),
                new { id = recipe.Id }, recipe);

        }
        


    }
}
