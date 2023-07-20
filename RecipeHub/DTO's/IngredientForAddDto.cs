using System.ComponentModel.DataAnnotations;

namespace RecipeHub.DTO_s;

public class IngredientForAddDto
{
    [Required] 
    [StringLength(128)]
    public string Name { get; set; } = string.Empty;
}