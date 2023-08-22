using System.ComponentModel.DataAnnotations;
using RecipeHub.Domain;

namespace RecipeHub.DTO_s;

public class RecipeByIngredientsDTO
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int PreparationTimeMin { get; set; }
    public int PreparationTimeMax { get; set; }
    public int Calories { get; set; }
}