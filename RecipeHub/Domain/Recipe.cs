using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeHub.Domain
{
    public class Recipe
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int PreparationTimeMin { get; set; }
        public int PreparationTimeMax { get; set; }
        [MaxLength(5000)]
        public string IngredientsText { get; set; }
        [MaxLength(5000)]
        public string RecipeText { get; set; }
        public int Calories { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        [ForeignKey(nameof(Ingredient))]
        public List<Ingredient> Ingredients { get; set; } = new();
    }
}
