﻿using System.ComponentModel.DataAnnotations;

namespace RecipeHub.DTO_s
{
    public class RecipesAllDto
    {
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public int PreparationTimeMin { get; set; }
        public int PreparationTimeMax { get; set; }
        [MaxLength(5000)]
        public string RecipeText { get; set; }
        public int Calories { get; set; }
        [Required]
        public string? UserId { get; set; }
    }
}
