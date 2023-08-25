using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories;

public interface IIngredientsRepository
{
    Task<IEnumerable<Ingredient>> GetIngredients(string? search);
    Task<Ingredient?> GetIngredient(int id);
    Task AddIngredient(Ingredient ingredient);
    Task<bool> UpdateIngredient(Ingredient ingredient);
    Task<bool> DeleteIngredient(int id);
}