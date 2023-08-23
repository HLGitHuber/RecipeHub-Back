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
        private readonly IRecipeRepository _recipeRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public RecipeController(RecipeDBContext context, IMapper mapper, IRecipeRepository recipeRepository, ILogger<Recipe> logger)
        {
            _mapper = mapper;
            _context = context;
            _recipeRepository = recipeRepository;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<RecipesAllDto>> GetAllRecipes()
        {
            _logger.LogInformation("Getting all recipes");
            return Ok(_context.Recipes);
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<RecipeDTO>> GetRecipeByID(int id)
        {
            _logger.LogInformation($"Getting recipe with id {id}");

            var recipe = _context.Recipes
                .Include(i=>i.Ingredients)
                .FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                _logger.LogWarning($"Recipe with id {id} wasn't found");
                return NotFound();
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
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddRecipe([FromBody] RecipeForAddDto recipeForAddDto)
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

            _context.Recipes.Add(recipe);
            _context.SaveChangesAsync();

            _logger.LogInformation($"New recipe added with id {recipe.Id}");


            return CreatedAtAction(nameof(GetAllRecipes),
                new { id = recipe.Id }, recipe);

        }

        [HttpGet("by-ingredients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<RecipeByIngredientsDTO>> GetRecipesByIngredients([FromQuery] List<int> ingredientIds)
        {
            var recipes = _recipeRepository.GetRecipesByIngredients(ingredientIds);

            if (recipes == null)
            {
                return NotFound();
            }

            var recipesDto = _mapper.Map<IEnumerable<RecipeByIngredientsDTO>>(recipes);
            
            return Ok(recipesDto);
        }

    }
}
