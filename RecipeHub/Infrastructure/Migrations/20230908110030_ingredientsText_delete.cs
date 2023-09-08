using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ingredientsText_delete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "191981b6-fedd-4509-a0b2-f0b3a015f5d0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9994c74-196f-4d09-a4f7-7a5f71f84774");

            migrationBuilder.DropColumn(
                name: "IngredientsText",
                table: "Recipes");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a4c612ea-7584-4bc6-a1fa-82ca5aca4039", null, "Admin", "ADMIN" },
                    { "a8ce56d6-6385-47e0-8dad-26ecf99663d4", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4c612ea-7584-4bc6-a1fa-82ca5aca4039");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8ce56d6-6385-47e0-8dad-26ecf99663d4");

            migrationBuilder.AddColumn<string>(
                name: "IngredientsText",
                table: "Recipes",
                type: "character varying(5000)",
                maxLength: 5000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "191981b6-fedd-4509-a0b2-f0b3a015f5d0", null, "User", "USER" },
                    { "f9994c74-196f-4d09-a4f7-7a5f71f84774", null, "Admin", "ADMIN" }
                });
        }
    }
}
