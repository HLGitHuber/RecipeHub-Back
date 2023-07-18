using RecipeHub.Domain;
using System.ComponentModel.DataAnnotations;

namespace RecipeHub.DTO_s
{
    public class RecipeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PreparationTimeMin { get; set; }
        public int PreparationTimeMax { get; set; }
        public string IngredientsText { get; set; }
        public string RecipeText { get; set; }
        public int Calories { get; set; }
        public User Owner { get; set; }
    }
}
