﻿using AutoMapper;
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

        public RecipeController(RecipeDBContext context, IMapper mapper, IRecipeRepository recipeRepository)
        {
            _mapper = mapper;
            _context = context;
            _recipeRepository = recipeRepository;
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
            var recipe = _context.Recipes
                .Include(i=>i.Ingredients)
                .FirstOrDefault(r => r.Id == id);
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

        [HttpGet("by-ingredients")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<RecipeByIngredientsDTO>> GetRecipesByIngredients([FromQuery] List<string> ingredientNames)
        {
            var recipes = _recipeRepository.GetRecipesByIngredients(ingredientNames);

            // Check if recipes is null before attempting to access properties
            if (recipes == null)
            {
                // Handle the case where recipes is null, e.g., return NotFound()
                return NotFound();
            }

            // Continue processing only if recipes is not null
            var filteredRecipes = recipes
                .Where(r => r.Ingredients != null && r.Ingredients.Any(ri => ri.Ingredient != null && ingredientNames.Contains(ri.Ingredient.Name)))
                .ToList();

            return Ok(filteredRecipes);
        }

    }
}
