using RecipeHub.Domain;

namespace RecipeHub.DTO_s
{
    public class RecipeIngredientDTO
    {
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
