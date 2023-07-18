using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Controllers;

[Route("api/ingredients")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientsRepository _repository;
    private readonly IMapper _mapper;

    public IngredientsController(IIngredientsRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper;
    }
    
    // GET api/ingredients
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<IngredientDto>> GetIngredients([FromQuery] string? search)
    {
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
        var ingredient = _repository.GetIngredient(id);

        if (ingredient is null)
        {
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
        if (string.IsNullOrWhiteSpace(ingredientForAddDto.Name))
        {
            ModelState.AddModelError("emptyName", "Name cannot be empty!");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ingredient = _mapper.Map<Ingredient>(ingredientForAddDto);

        _repository.AddIngredient(ingredient);

        var ingredientDto = _mapper.Map<IngredientDto>(ingredient);

        return CreatedAtAction(nameof(GetIngredient),
            new { id = ingredient.Id }, ingredientDto);
    }
}