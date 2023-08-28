using AutoMapper;
using AutoMapper.Internal;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories
{
    public class RecipeRepository : IRecipeRepository
    {
        private readonly RecipeDBContext _dbContext;
        private readonly IMapper _mapper;

        public RecipeRepository(RecipeDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper;
        }
        
        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            return await _dbContext.Recipes.ToListAsync();
        }

        public async Task<Recipe?> GetRecipe(int id)
        {
            return await _dbContext.Recipes
                .Include(i=>i.Ingredients)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task AddRecipe(Recipe recipe)
        {
            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();
        }

        public bool UpdateRecipe(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> DeleteRecipe(int recipeid)
        {
            var recipe = await _dbContext
                .Recipes
                .FirstOrDefaultAsync(r=>r.Id == recipeid);
            if (recipe is null)
            {
                return false;
            }
            _dbContext .Recipes .Remove(recipe);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<IEnumerable<Recipe>> GetRecipesByIngredientIDs(List<int> ingredientIDs)
        {
            return await _dbContext.Recipes
                .Where(recipe => recipe.Ingredients.All(ri => ingredientIDs.Contains(ri.IngredientId)))
                .ToListAsync();
        }
        
    }
}
