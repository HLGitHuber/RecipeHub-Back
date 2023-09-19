using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a4c612ea-7584-4bc6-a1fa-82ca5aca4039");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a8ce56d6-6385-47e0-8dad-26ecf99663d4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "208a70b3-5fa6-4649-997c-7bc22e67c5b9", null, "User", "USER" },
                    { "ae4ef077-a1b6-4d51-b85f-3a89ab1a4200", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "208a70b3-5fa6-4649-997c-7bc22e67c5b9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae4ef077-a1b6-4d51-b85f-3a89ab1a4200");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a4c612ea-7584-4bc6-a1fa-82ca5aca4039", null, "Admin", "ADMIN" },
                    { "a8ce56d6-6385-47e0-8dad-26ecf99663d4", null, "User", "USER" }
                });
        }
    }
}
