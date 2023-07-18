using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace RecipeHub.Domain
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(100)]
        public string Password { get; set; }
        public List<Recipe> OwnedRecipes { get; set; } = new();
        public List<Recipe> FavouritedRecipes { get; set; } = new();
    }
}
