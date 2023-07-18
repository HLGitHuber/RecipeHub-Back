using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure;

namespace RecipeHub.Controllers
{
    [Route("api/Recipe")]
    [ApiController]
    public class RecipeController: ControllerBase
    {
        private readonly RecipeDBContext _context;
        public RecipeController(RecipeDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<IEnumerable<RecipeDTO>> GetAllRecipes()
        {
            return Ok(_context.Recipes);
        }
        [HttpGet("{id:int}")]
        public ActionResult<IEnumerable<RecipeDTO>> GetRecipeByID(int id)
        {
            var recipe = _context.Recipes.FirstOrDefault(r => r.Id == id);
            if (recipe == null)
            {
                return NoContent();
            }
            var recipeDto = new RecipeDTO()
            {
                Id = recipe.Id,
                Name = recipe.Name,
                PreparationTimeMax = recipe.PreparationTimeMax,
            };
            return Ok(recipeDto);
        }


    }
}
