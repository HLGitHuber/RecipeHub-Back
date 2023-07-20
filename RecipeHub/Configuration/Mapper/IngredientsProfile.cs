using AutoMapper;
using RecipeHub.Domain;
using RecipeHub.DTO_s;

namespace RecipeHub.Configuration.Mapper;

public class IngredientsProfile : Profile
{
    public IngredientsProfile()
    {
        CreateMap<Ingredient, IngredientDto>();
        CreateMap<IngredientForAddDto, Ingredient>();
        CreateMap<IngredientForUpdateDto, Ingredient>();
    }
}