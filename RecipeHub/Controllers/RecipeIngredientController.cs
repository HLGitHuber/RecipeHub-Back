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

        private readonly IRecipeIngredientRepository _recipeIngredientRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RecipeIngredientController(RecipeDBContext context, IRecipeIngredientRepository recipeIngredientRepository, IMapper mapper, ILogger<RecipeIngredient> logger)
        {
            _context = context;
            _mapper = mapper;
            _recipeIngredientRepository = recipeIngredientRepository;
            _logger = logger;
        }




        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<int>>> GetAllIngredientIdsByRecipeId(int id)
        {
            _logger.LogInformation($"Getting ingredients with id {id}");

            var ingredientIds = await _recipeIngredientRepository.GetAllIngredientIdsByRecipeId(id);

            if (ingredientIds is null)
            {
                _logger.LogWarning($"No ingredients were found with id {id}");
                return NotFound();
            }

            return Ok(ingredientIds);
        }

        [HttpGet("ingredientnames/{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<string>>> GetIngredientNamesForRecipeId(int id)
        {
            _logger.LogInformation($"Getting ingredients names for recipe with id {id}");

            var ingredientNames = await _recipeIngredientRepository.GetIngredientNamesForRecipeId(id);


            if (ingredientNames is null)
            {
                _logger.LogWarning($"No ingredients names for recipe with id {id} were found");
                return NotFound();
            }

            return Ok(ingredientNames);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddIngredientToRecipe([FromQuery] RecipeIngredientForAddDto recipeIngredientForAddDTO)
        {
            _logger.LogInformation("Adding new ingredient for recipe");

            var newRecipeIngredient = await _recipeIngredientRepository.AddIngredientToRecipe(recipeIngredientForAddDTO);

            if (newRecipeIngredient == null)
            {
                _logger.LogWarning($"Recipe wasn't found because of bad request");
                return BadRequest("RecipeIngredient data is null.");
            }

            return NoContent();
        }

        [HttpDelete("deleteallingredients/{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAllIngredientsForRecipe(int recipeId)
        {
            _logger.LogInformation($"Deleting all ingredients for recipe with id {recipeId}");

            var deleteAllIngredients = await _recipeIngredientRepository.DeleteAllIngredientsForRecipe(recipeId);
            if (!deleteAllIngredients)
            {
                _logger.LogWarning($"Recipe wasn't found");
                return NotFound();
            }
            return NoContent();
        }

        [HttpDelete("deletesingleingredient")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> DeleteSingleIngredientFromRecipe([FromQuery] int recipeid, int ingredientid)
        {
            var recipeIngredient = await _recipeIngredientRepository.DeleteSingleIngredientFromRecipe(recipeid, ingredientid);

            if (recipeIngredient == false)
            {
                return NotFound();
            }
            return NoContent();
        }


    }
}
