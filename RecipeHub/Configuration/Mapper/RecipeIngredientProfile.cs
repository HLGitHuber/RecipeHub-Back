using AutoMapper;
using RecipeHub.Domain;
using RecipeHub.DTO_s;

namespace RecipeHub.Configuration.Mapper
{
    public class RecipeIngredientProfile:Profile
    {
        public RecipeIngredientProfile()
        {
            CreateMap<RecipeIngredient, RecipeByIngredientsDTO>();
            CreateMap<RecipeIngredient, RecipeIngredientForAddDto>();
        }
    }
}
