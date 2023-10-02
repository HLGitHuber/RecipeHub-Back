using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Controllers
{
    [Route("api/favourites")]
    public class UserFavouriteRecipesController : ControllerBase
    {
        private readonly IUserFavouriteRecipesRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public UserFavouriteRecipesController(IUserFavouriteRecipesRepository repository, IMapper mapper, ILogger<Ingredient> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUserFavouriteRecipes(string userId)
        {
            _logger.LogInformation($"Getting all favourite recipes for user with id {userId}");

            var favouriteRecipes = await _repository.GetUserFavouriteRecipesAsync(userId);
            var recipesAllDto = _mapper.Map<IEnumerable<RecipesAllDto>>(favouriteRecipes);
            return Ok(recipesAllDto);
        }

        [HttpPost("{userId}/add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRecipeToUserFavourites(string userId, [FromBody] int recipeId)
        {
            _logger.LogInformation($"Adding new recipe to user with id {userId} favourites");

            if (!ModelState.IsValid)
            {
                _logger.LogError("Adding ingredient failed because of bad request");

                return BadRequest(ModelState);
            }

            await _repository.AddRecipeToUserFavouritesAsync(userId, recipeId);

            _logger.LogInformation($"Recipe with id {recipeId} added to favourites of user with id {userId}");

            return Ok();
        }

        [HttpDelete("{userId}/delete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRecipeFromUserFavourites(string userId, [FromBody] int recipeId)
        {
            _logger.LogInformation($"Deleting recipe with id {recipeId} from user with id {userId} favourites");

            var success = await _repository.DeleteRecipeFromUserFavouritesAsync(userId, recipeId);

            if (!success)
            {
                _logger.LogInformation($"Attempt to delete recipe with ID {recipeId} for user with {userId} failed");
                return NotFound();
            }
            _logger.LogInformation($"Recipe with ID {recipeId} deleted from favourites of user with {userId}");
            return NoContent();
        }
    }
}