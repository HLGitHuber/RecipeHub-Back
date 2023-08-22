using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Controllers;

[Route("api/ingredients")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientsRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public IngredientsController(IIngredientsRepository repository, IMapper mapper, ILogger<Ingredient> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper;
        _logger = logger;
    }
    
    // GET api/ingredients
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<IngredientDto>> GetIngredients([FromQuery] string? search)
    {
        _logger.LogInformation("Getting all ingredients");

        var ingredients = _repository.GetIngredients(search);

        var ingredientsDto = _mapper.Map<IEnumerable<IngredientDto>>(ingredients);

        return Ok(ingredientsDto);
    }
    
    // GET api/ingredients/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IngredientDto> GetIngredient(int id)
    {
        _logger.LogInformation($"Getting ingredient with id {id}");

        var ingredient = _repository.GetIngredient(id);

        if (ingredient is null)
        {
            _logger.LogWarning($"Ingredient with id {id} wasn't found");
            return NotFound();
        }

        var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

        return Ok(ingredientDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult AddIngredient([FromBody] IngredientForAddDto ingredientForAddDto)
    {
        _logger.LogInformation("Adding new ingredient");

        if (string.IsNullOrWhiteSpace(ingredientForAddDto.Name))
        {
            ModelState.AddModelError("emptyName", "Name cannot be empty!");
        }

        if (!ModelState.IsValid)
        {
            _logger.LogError("Adding ingredient failed because of bad request");

            return BadRequest(ModelState);
        }

        var ingredient = _mapper.Map<Ingredient>(ingredientForAddDto);

        _repository.AddIngredient(ingredient);

        _logger.LogInformation($"New ingredient added with id {ingredient.Id}");

        var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

        return CreatedAtAction(nameof(GetIngredient),
            new { id = ingredient.Id }, ingredientDto);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateIngredient(int id, [FromBody] IngredientForUpdateDto ingredientForUpdateDto)
    {
        _logger.LogInformation($"Updating ingredient with id {id}");

        var ingredient = _mapper.Map<Ingredient>(ingredientForUpdateDto);
        ingredient.Id = id;

        var success = _repository.UpdateIngredient(ingredient);

        if (!success)
        {
            _logger.LogError("Updating ingredient wasn't found");

            return NotFound();
        }

        _logger.LogInformation($"Ingredient with id {id} correctly updated");

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteIngredient(int id)
    {
        _logger.LogInformation($"Deleting ingredient with id {id}");

        var success = _repository.DeleteIngredient(id);

        if (!success)
        {
            _logger.LogError("Deleting ingredient wasn't found");

            return NotFound();
        }

        _logger.LogInformation($"Ingredient with id {id} correctly deleted");

        return NoContent();
    }
}