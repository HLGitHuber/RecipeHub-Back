﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeHub.Domain;
using RecipeHub.DTO_s;
using RecipeHub.Infrastructure.Repositories;

namespace RecipeHub.Infrastructure.Repositories
{
    public class RecipeIngredientRepository: IRecipeIngredientRepository
    {
        private readonly RecipeDBContext _dbContext;
        private readonly IMapper _mapper;

        public RecipeIngredientRepository(RecipeDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _mapper = mapper;
        }

        public async Task<ActionResult<IEnumerable<int>>> GetAllIngredientIdsByRecipeId(int id)
        {
            return await _dbContext.RecipeIngredients
                .Where(i => i.RecipeId == id)
                .Select(i => i.IngredientId)
                .ToListAsync();
        }
        public async Task<ActionResult<List<string>>> GetIngredientNamesForRecipeId(int id)
        {
            var ingredientNames = await _dbContext.RecipeIngredients
                .Where(i => i.RecipeId == id)
                .Select(i => i.Ingredient.Name)
                .ToListAsync();

            return (ingredientNames);
        }
        public async Task<RecipeIngredient> AddIngredientToRecipe([FromQuery] RecipeIngredientForAddDto recipeIngredientForAddDTO)
        {
            var recipe = await _dbContext.Recipes.FindAsync(recipeIngredientForAddDTO.RecipeId);
            var ingredient = await _dbContext.Ingredients.FindAsync(recipeIngredientForAddDTO.IngredientId);

            var newRecipeIngredient = new RecipeIngredient
            {
                Recipe = recipe,
                Ingredient = ingredient
            };

            await _dbContext.RecipeIngredients.AddAsync(newRecipeIngredient);
            await _dbContext.SaveChangesAsync();

            return newRecipeIngredient;
        }
        public async Task<bool> DeleteAllIngredientsForRecipe(int recipeid)
        {
            var recipeIngredients = await _dbContext.RecipeIngredients
            .Where(ri => ri.RecipeId == recipeid)
            .ToListAsync();

            _dbContext.RecipeIngredients.RemoveRange(recipeIngredients);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteSingleIngredientFromRecipe([FromQuery] int recipeid, int ingredientid)
        {
            var recipeIngredient = await _dbContext.RecipeIngredients
                .FirstOrDefaultAsync(ri => ri.RecipeId == recipeid && ri.IngredientId == ingredientid);
            if (recipeIngredient == null) 
            {
                return false;
            }

            _dbContext.RecipeIngredients.Remove(recipeIngredient);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
