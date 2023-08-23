using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Controllers;

[Route("api/ingredients")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientsRepository _repository;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public IngredientsController(IIngredientsRepository repository, IMapper mapper, IMemoryCache memoryCache)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper;
        _memoryCache = memoryCache;
    }
    
    // GET api/ingredients
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ResponseCache(CacheProfileName = "Any-60")]
    public ActionResult<IEnumerable<IngredientDto>> GetIngredients([FromQuery] string? search)
    {
        var cacheKey = $"{nameof(IngredientsController)}-{nameof(GetIngredients)}";

        if (!_memoryCache.TryGetValue<IEnumerable<IngredientDto>>(cacheKey, out var ingredientsDto))
        {
            var ingredients = _repository.GetIngredients(search);

            if (ingredients is not null)
            {
                ingredientsDto = _mapper.Map<IEnumerable<IngredientDto>>(ingredients);

                _memoryCache.Set(cacheKey, ingredientsDto, TimeSpan.FromSeconds(60));
            }
        }

        if (ingredientsDto is null)
        {
            return NotFound();
        }

        return Ok(ingredientsDto);
    }
    
    // GET api/ingredients/{id}
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ResponseCache(CacheProfileName = "Any-60")]
    public ActionResult<IngredientDto> GetIngredient(int id)
    {
        var cacheKey = $"{nameof(IngredientsController)}-{nameof(GetIngredient)}-{id}";

        if (!_memoryCache.TryGetValue<IngredientDto>(cacheKey, out var ingredientDto))
        {

            var ingredient = _repository.GetIngredient(id);
            
            if (ingredient is not null)
            {
                ingredientDto = _mapper.Map<IngredientDto>(ingredient);

                _memoryCache.Set(cacheKey, ingredientDto, TimeSpan.FromSeconds(60));
            }
        }

        if (ingredientDto is null)
        {
            return NotFound();
        }

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

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult UpdateIngredient(int id, [FromBody] IngredientForUpdateDto ingredientForUpdateDto)
    {
        var ingredient = _mapper.Map<Ingredient>(ingredientForUpdateDto);
        ingredient.Id = id;

        var success = _repository.UpdateIngredient(ingredient);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteIngredient(int id)
    {
        var success = _repository.DeleteIngredient(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}