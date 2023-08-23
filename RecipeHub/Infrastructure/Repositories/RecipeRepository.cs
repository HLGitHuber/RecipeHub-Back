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
        
        public IEnumerable<Recipe> GetRecipe(string? search)
        {
            throw new NotImplementedException();
        }

        public Recipe? GetRecipe(int id)
        {
            throw new NotImplementedException();
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
            var query = _dbContext.Recipes.AsQueryable();

            if (ingredientIds != null && ingredientIds.Any())
            {
                query = query.Where(recipe => recipe.Ingredients
                    .Any(ri => ingredientIds.Contains(ri.IngredientId)));
            }

            var recipes = query.ToList(); // Fetch the recipes that match the criteria

            // Manually load the Ingredients for each Recipe
            foreach (var recipe in recipes)
            {
                recipe.Ingredients = _dbContext.RecipeIngredients
                    .Where(ri => ri.RecipeId == recipe.Id)
                    .ToList(); // Load the RecipeIngredients
            }

            return recipes;
        }




    }
}
