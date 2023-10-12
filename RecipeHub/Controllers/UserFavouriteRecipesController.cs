using System;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Authorization;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure.Repositories;
using System.Security.Claims;


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

        [HttpPost("add/{recipeId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddRecipeToUserFavourites(int recipeId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!ModelState.IsValid)
            {
                _logger.LogError("Adding ingredient failed because of bad request");

                return BadRequest(ModelState);
            }

            await _repository.AddRecipeToUserFavouritesAsync(userIdClaim!, recipeId);

            _logger.LogInformation($"Recipe with id {recipeId} added to favourites of user with id {userIdClaim}");

            return Ok();
        }

        [HttpDelete("delete/{recipeId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteRecipeFromUserFavourites(int recipeId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            _logger.LogInformation($"Deleting recipe with id {recipeId} from user with id {userIdClaim} favourites");

            var success = await _repository.DeleteRecipeFromUserFavouritesAsync(userIdClaim!, recipeId);

            if (!success)
            {
                _logger.LogInformation($"Attempt to delete recipe with ID {recipeId} for user with {userIdClaim} failed");
                return NotFound();
            }
            _logger.LogInformation($"Recipe with ID {recipeId} deleted from favourites of user with {userIdClaim}");
            return NoContent();
        }
    }
}