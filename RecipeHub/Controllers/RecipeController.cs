using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    //[Authorize]
    public class RecipeController: ControllerBase
    {
        private readonly IRecipeRepository _recipeRepository;
        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RecipeController(IMapper mapper, IRecipeRepository recipeRepository,IRecipeIngredientRepository recipeIngredientRepository, ILogger<Recipe> logger)
        {
            _mapper = mapper;
            _recipeRepository = recipeRepository;
            _recipeIngredientRepository = recipeIngredientRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<RecipesAllDto>>> GetAllRecipes()
        {
            _logger.LogInformation("Getting all recipes");

            var recipes = await _recipeRepository.GetRecipes();

            return Ok(recipes);
        }
        
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RecipeDTO>> GetRecipeById(int id)
        {
            _logger.LogInformation($"Getting recipe with id {id}");

            var recipe = await _recipeRepository.GetRecipe(id);
            
            if (recipe == null)
            {
                _logger.LogWarning($"Recipe with id {id} not found");
                return NotFound();
            }
            
            var recipeDto = _mapper.Map<RecipeDTO>(recipe);
            
            return Ok(recipeDto);
        }

        [HttpGet("recipesbyingredients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<RecipeDTO>>> GetRecipesByIngredientIDs([FromQuery] List<int> ingredientIDs)
        {
            _logger.LogInformation($"Getting recipes by ingredient id");
            if (ingredientIDs == null || ingredientIDs.Count == 0)
            {
                _logger.LogInformation($"No ingredient IDs provided");
                return BadRequest("No ingredient IDs provided.");
            }

            var recipes = await _recipeRepository.GetRecipesByIngredientIDs(ingredientIDs);

            var recipesDto = _mapper.Map<IEnumerable<RecipeByIngredientsDTO>>(recipes);
            
            return Ok(recipesDto);
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRecipe([FromBody] RecipeForAddDto recipeForAddDto)
        {
            _logger.LogInformation("Adding new recipe");

            if (string.IsNullOrWhiteSpace(recipeForAddDto.Name))
            {
                ModelState.AddModelError("emptyName", "Name cannot be empty!");
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError("Adding recipe failed because of bad request");
                return BadRequest(ModelState);
            }

            var recipe = _mapper.Map<Recipe>(recipeForAddDto);

            await _recipeRepository.AddRecipe(recipe);

            _logger.LogInformation($"New recipe added with id {recipe.Id}");


            return CreatedAtAction(nameof(GetAllRecipes),
                new { id = recipe.Id }, recipe);

        }

        //Update
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateRecipe(int id, [FromBody] RecipeForUpdateDto recipeForUpdateDto)
        {
            _logger.LogInformation($"Updating recipe with ID {id}");
            var recipe = _mapper.Map<Recipe>(recipeForUpdateDto);
            recipe.Id = id;

            var success = await _recipeRepository.UpdateRecipe(recipe);
            if (!success)
            {
                _logger.LogInformation($"Attempt to update recipe with ID {id} failed - no recipe found");
                return NotFound();
            }
            _logger.LogInformation($"Recipe with ID {id} has been updated");
            return NoContent();

        }


        // DELETE api/recipe/1
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            _logger.LogInformation($"Attempt to delete recipe with ID {id}");
            var deleteRecipe = await _recipeRepository.DeleteRecipe(id);
            var deleteRecipeIngredients = await _recipeIngredientRepository.DeleteAllIngredientsForRecipe(id);
            if (!deleteRecipe)
            {
                _logger.LogInformation($"Attempt to delete recipe with ID {id} failed - no recipe found");
                return NotFound();
            }
            _logger.LogInformation($"Recipe with ID {id} deleted");
            return NoContent();
        }
    }
}
