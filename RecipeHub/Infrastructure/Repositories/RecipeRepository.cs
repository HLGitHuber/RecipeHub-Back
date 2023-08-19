using AutoMapper;
using AutoMapper.Internal;
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

        public IEnumerable<Recipe> GetRecipesByIngredients(List<string>? ingredientNames)
        {
            var query = _dbContext.Recipes.AsQueryable();

            if (ingredientNames != null && ingredientNames.Any())
            {
                query = query.Where(recipe => ingredientNames
                    .Any(ingredientName => recipe.Name
                        .Contains(ingredientName)));

            }

            return query.ToList();
        }
    }
}
