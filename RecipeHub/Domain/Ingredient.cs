using System.ComponentModel.DataAnnotations;

namespace RecipeHub.Domain
{
    public class Ingredient
    {
        [Key]
        [MaxLength(100)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
