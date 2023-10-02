using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;

namespace RecipeHub.Domain
{
    public class User: IdentityUser
    {
        public ICollection<UserFavouriteRecipe> FavouriteRecipes { get; set; }
    }
}
