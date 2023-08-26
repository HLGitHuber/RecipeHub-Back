using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipeHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userrecipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recipes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Recipes",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipes_AspNetUsers_UserId",
                table: "Recipes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipes_AspNetUsers_UserId",
                table: "Recipes");

            migrationBuilder.DropIndex(
                name: "IX_Recipes_UserId",
                table: "Recipes");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Recipes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "Calories", "IngredientsText", "Name", "PreparationTimeMax", "PreparationTimeMin", "RecipeText", "UserId" },
                values: new object[] { 2, 1000, "IngredientsText", "Milk with cheese", 2, 1, "RecipeText", 0 });
        }
    }
}
