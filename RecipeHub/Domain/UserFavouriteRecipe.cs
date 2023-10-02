namespace RecipeHub.Domain
{
    public class UserFavouriteRecipe
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}