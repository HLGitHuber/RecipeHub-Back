using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories;

public class IngredientsRepository : IIngredientsRepository
{
    private readonly RecipeDBContext _dbContext;
    private readonly IMapper _mapper;

    public IngredientsRepository(RecipeDBContext dbContext, Mapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper;
    }
    
    public IEnumerable<Ingredient> GetIngredients(string? search)
    {
        var query = _dbContext.Ingredients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        return query.ToList();
    }

    public Ingredient? GetIngredient(int id)
    {
        return _dbContext.Ingredients.FirstOrDefault(i => i.Id == id);
    }

    public void AddIngredient(Ingredient ingredient)
    {
        _dbContext.Ingredients.Add(ingredient);
        _dbContext.SaveChanges();
    }

    public bool UpdateIngredient(Ingredient ingredient)
    {
        var ingredientFromDb = _dbContext
            .Ingredients
            .FirstOrDefault(i => i.Id == ingredient.Id);

        if (ingredientFromDb is null)
        {
            return false;
        }

        ingredientFromDb = _mapper.Map<Ingredient>(ingredient);

        _dbContext.SaveChanges();

        return true;
    }

    public bool DeleteIngredient(int id)
    {
        var ingredient = _dbContext
            .Ingredients
            .FirstOrDefault(i => i.Id == id);

        if (ingredient is null)
        {
            return false;
        }

        _dbContext.Ingredients.Remove(ingredient);
        _dbContext.SaveChanges();

        return true;
    }
}