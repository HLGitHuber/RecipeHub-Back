using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace RecipeHub.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deletecookies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "31421b0b-1070-41e2-b523-cfdcb4ef79de");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b15cdcd7-6750-49b2-a351-bd7615b0d95d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "191981b6-fedd-4509-a0b2-f0b3a015f5d0", null, "User", "USER" },
                    { "f9994c74-196f-4d09-a4f7-7a5f71f84774", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "191981b6-fedd-4509-a0b2-f0b3a015f5d0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9994c74-196f-4d09-a4f7-7a5f71f84774");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "31421b0b-1070-41e2-b523-cfdcb4ef79de", null, "User", "USER" },
                    { "b15cdcd7-6750-49b2-a351-bd7615b0d95d", null, "Admin", "ADMIN" }
                });
        }
    }
}
