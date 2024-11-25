using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TournamentPlanner.Identity.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDefaultDomainRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "151f44f2-8c1d-4076-87a4-c0a6a116ecaf", null, "User", "USER" },
                    { "a7f72609-61bc-4acc-96bb-4b623b9273cf", null, "Player", "PLAYER" },
                    { "ac031ae6-5262-4833-ba93-f67122748ac5", null, "Admin", "ADMIN" },
                    { "f0a248bb-fe6d-4834-a614-c9a5d31ae8b2", null, "Moderator", "MODERATOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "151f44f2-8c1d-4076-87a4-c0a6a116ecaf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a7f72609-61bc-4acc-96bb-4b623b9273cf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac031ae6-5262-4833-ba93-f67122748ac5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f0a248bb-fe6d-4834-a614-c9a5d31ae8b2");
        }
    }
}
