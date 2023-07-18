using System.ComponentModel.DataAnnotations;

namespace RecipeHub.DTO_s;

public class IngredientForUpdateDto
{
    [Required]
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;
}