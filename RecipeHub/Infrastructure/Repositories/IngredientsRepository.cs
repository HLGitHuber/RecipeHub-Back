using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories;

public class IngredientsRepository : IIngredientsRepository
{
    private readonly RecipeDBContext _dbContext;
    private readonly IMapper _mapper;

    public IngredientsRepository(RecipeDBContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<Ingredient>> GetIngredients(string? search)
    {
        var query = _dbContext.Ingredients.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(c => c.Name.Contains(search));
        }

        return await query.ToListAsync();
    }

    public async Task<Ingredient?> GetIngredient(int id)
    {
        return await _dbContext.Ingredients.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task AddIngredient(Ingredient ingredient)
    {
        await _dbContext.Ingredients.AddAsync(ingredient);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> UpdateIngredient(Ingredient ingredient)
    {
        var ingredientFromDb = await _dbContext
            .Ingredients
            .FirstOrDefaultAsync(i => i.Id == ingredient.Id);

        if (ingredientFromDb is null)
        {
            return false;
        }

        ingredientFromDb.Name = ingredient.Name;
        
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteIngredient(int id)
    {
        var ingredient = await _dbContext
            .Ingredients
            .FirstOrDefaultAsync(i => i.Id == id);

        if (ingredient is null)
        {
            return false;
        }

        _dbContext.Ingredients.Remove(ingredient);
        await _dbContext.SaveChangesAsync();

        return true;
    }
}