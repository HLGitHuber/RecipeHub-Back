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
        public string RecipeText { get; set; }
        public int Calories { get; set; }
        public User? User { get; set; }
        [ForeignKey(nameof(User))]
        public string? UserId { get; set; }
        [Required]
        public ICollection<RecipeIngredient> Ingredients { get; set; }
    }
}
