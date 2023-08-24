﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using RecipeHub.Infrastructure;

#nullable disable

namespace RecipeHub.Migrations
{
    [DbContext(typeof(RecipeDBContext))]
    partial class RecipeDBContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("RecipeHub.Domain.Ingredient", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Ingredients", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Milk"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Butter"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Cheese"
                        });
                });

            modelBuilder.Entity("RecipeHub.Domain.Recipe", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Calories")
                        .HasColumnType("integer");

                    b.Property<string>("IngredientsText")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("PreparationTimeMax")
                        .HasColumnType("integer");

                    b.Property<int>("PreparationTimeMin")
                        .HasColumnType("integer");

                    b.Property<int?>("Recipe")
                        .HasColumnType("integer");

                    b.Property<string>("RecipeText")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("character varying(5000)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("Recipe");

                    b.ToTable("Recipes");

                    b.HasData(
                        new
                        {
                            Id = 2,
                            Calories = 1000,
                            IngredientsText = "IngredientsText",
                            Name = "Milk with cheese",
                            PreparationTimeMax = 2,
                            PreparationTimeMin = 1,
                            RecipeText = "RecipeText",
                            UserId = 0
                        });
                });

            modelBuilder.Entity("RecipeHub.Domain.RecipeIngredient", b =>
                {
                    b.Property<int>("RecipeId")
                        .HasColumnType("integer");

                    b.Property<int>("IngredientId")
                        .HasColumnType("integer");

                    b.HasKey("RecipeId", "IngredientId");

                    b.HasIndex("IngredientId");

                    b.ToTable("RecipeIngredients");

                    b.HasData(
                        new
                        {
                            RecipeId = 2,
                            IngredientId = 1
                        },
                        new
                        {
                            RecipeId = 2,
                            IngredientId = 3
                        });
                });

            modelBuilder.Entity("RecipeHub.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("RecipeHub.Domain.Recipe", b =>
                {
                    b.HasOne("RecipeHub.Domain.User", null)
                        .WithMany("OwnedRecipes")
                        .HasForeignKey("Recipe");
                });

            modelBuilder.Entity("RecipeHub.Domain.RecipeIngredient", b =>
                {
                    b.HasOne("RecipeHub.Domain.Ingredient", "Ingredient")
                        .WithMany("Recipes")
                        .HasForeignKey("IngredientId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("RecipeHub.Domain.Recipe", "Recipe")
                        .WithMany("Ingredients")
                        .HasForeignKey("RecipeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Ingredient");

                    b.Navigation("Recipe");
                });

            modelBuilder.Entity("RecipeHub.Domain.Ingredient", b =>
                {
                    b.Navigation("Recipes");
                });

            modelBuilder.Entity("RecipeHub.Domain.Recipe", b =>
                {
                    b.Navigation("Ingredients");
                });

            modelBuilder.Entity("RecipeHub.Domain.User", b =>
                {
                    b.Navigation("OwnedRecipes");
                });
#pragma warning restore 612, 618
        }
    }
}
