using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace RecipeHub.Domain
{
    public class User: IdentityUser
    {
        //[Key] // not necessary - inherits from IdentityUSer

        //public int Id { get; set; }
        //[Required]
        //[MaxLength(100)]
        //public string Name { get; set; }
        //[Required]
        //[EmailAddress]
        //[MaxLength(100)]
        //public string Email { get; set; }
        //[Required]
        //[MaxLength(100)]
        //public string PasswordHash { get; set; }
        //[Required]
        //[MaxLength(100)]
        //public string PasswordSalt { get; set; }

        //[Required]
        //public Role Role { get; set; }
        //[ForeignKey(nameof(Recipe))]
        //public List<Recipe> OwnedRecipes { get; set; } = new();
        //public List<Recipe> FavouritedRecipes { get; set; } = new();
    }
}
