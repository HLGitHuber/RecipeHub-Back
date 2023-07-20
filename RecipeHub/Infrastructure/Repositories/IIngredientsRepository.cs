using RecipeHub.Domain;

namespace RecipeHub.Infrastructure.Repositories;

public interface IIngredientsRepository
{
    IEnumerable<Ingredient> GetIngredients(string? search);
    Ingredient? GetIngredient(int id);
    void AddIngredient(Ingredient ingredient);
    bool UpdateIngredient(Ingredient ingredient);
    bool DeleteIngredient(int id);
}