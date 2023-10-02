using AutoMapper;
using RecipeHub.Domain;
using RecipeHub.DTO_s;

namespace RecipeHub.Configuration.Mapper
{
	public class UserFavouriteRecipesProfile : Profile
    {
		public UserFavouriteRecipesProfile()
		{
            CreateMap<UserFavouriteRecipe, RecipesAllDto>().ReverseMap();
        }
    }
}

