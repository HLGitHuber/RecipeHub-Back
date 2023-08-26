using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class userguid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "9f65cbc7-ad7d-402f-9319-7bdff5d031a9", null, "Admin", "ADMIN" },
                    { "afa8922a-9dc1-45d8-84e8-9e6858b2fb92", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9f65cbc7-ad7d-402f-9319-7bdff5d031a9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "afa8922a-9dc1-45d8-84e8-9e6858b2fb92");
        }
    }
}
