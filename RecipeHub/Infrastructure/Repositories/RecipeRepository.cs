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
        
        public IEnumerable<Recipe> GetRecipes()
        {
            return _dbContext.Recipes;
        }

        public Recipe? GetRecipe(int id)
        {
            return _dbContext.Recipes
                .Include(i=>i.Ingredients)
                .FirstOrDefault(r => r.Id == id);
        }

        public void AddRecipe(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public bool UpdateRecipe(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public bool DeleteRecipe(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Recipe> GetRecipesByIngredients(List<int>? ingredientIds)
        {
            if (ingredientIds == null || !ingredientIds.Any())
            {
                // If no ingredients are provided, return an empty list or handle it as needed.
                return new List<Recipe>();
            }

            var recipeIds = _dbContext.RecipeIngredients
                .Where(ri => ingredientIds.Contains(ri.IngredientId))
                .Select(ri => ri.RecipeId)
                .Distinct()
                .ToList(); // Materialize the query to execute it in memory

            var recipes = _dbContext.Recipes
                .Where(recipe => recipeIds.Contains(recipe.Id))
                .ToList(); // Materialize the query to execute it in memory
            


            return recipes;
        }







    }
}
