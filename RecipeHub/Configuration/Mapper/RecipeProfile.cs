﻿using AutoMapper;
using RecipeHub.Domain;
using RecipeHub.DTO_s;

namespace RecipeHub.Configuration.Mapper
{
    public class RecipeProfile: Profile
    {
        public RecipeProfile()
        {
            CreateMap<Recipe, RecipeDTO>().ReverseMap();
            CreateMap<Recipe, RecipesAllDto>().ReverseMap();
            CreateMap<Recipe, RecipeForAddDto>().ReverseMap();
            CreateMap<Recipe, RecipeByIngredientsDTO>();
            CreateMap<Recipe, RecipeForUpdateDto>().ReverseMap();
        }
    }
}
