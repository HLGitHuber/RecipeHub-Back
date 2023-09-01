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

        public RecipeIngredientController(RecipeDBContext context, IRecipeIngredientRepository recipeIngredientRepository, IMapper mapper, ILogger logger)
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
            var ingredientIds = await _recipeIngredientRepository.GetAllIngredientIdsByRecipeId(id);

            if (ingredientIds is null)
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
            var ingredientNames = await _recipeIngredientRepository.GetIngredientNamesForRecipeId(id);


            if (ingredientNames is null)
            {
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
            var newRecipeIngredient = await _recipeIngredientRepository.AddIngredientToRecipe(recipeIngredientForAddDTO);

            if (newRecipeIngredient == null)
            {
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
            var deleteAllIngredients = await _recipeIngredientRepository.DeleteAllIngredientsForRecipe(recipeId);
            if (!deleteAllIngredients)
            {

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
