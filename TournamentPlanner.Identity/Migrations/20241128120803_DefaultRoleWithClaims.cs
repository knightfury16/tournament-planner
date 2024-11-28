using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TournamentPlanner.Identity.Migrations
{
    /// <inheritdoc />
    public partial class DefaultRoleWithClaims : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "21fca900-4e5d-47fe-9605-bb02c2c94212", null, "Admin", "ADMIN" },
                    { "34b824a6-8bca-4760-b2cb-628224507b98", null, "Player", "PLAYER" },
                    { "d6aa04a7-3638-415d-8418-c5782e7333f3", null, "Moderator", "MODERATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permission", "Read", "d6aa04a7-3638-415d-8418-c5782e7333f3" },
                    { 2, "Permission", "Edit", "d6aa04a7-3638-415d-8418-c5782e7333f3" },
                    { 3, "Permission", "AddScore", "d6aa04a7-3638-415d-8418-c5782e7333f3" },
                    { 4, "Permission", "Read", "21fca900-4e5d-47fe-9605-bb02c2c94212" },
                    { 5, "Permission", "Edit", "21fca900-4e5d-47fe-9605-bb02c2c94212" },
                    { 6, "Permission", "Create", "21fca900-4e5d-47fe-9605-bb02c2c94212" },
                    { 7, "Permission", "Delete", "21fca900-4e5d-47fe-9605-bb02c2c94212" },
                    { 8, "Permission", "AddScore", "21fca900-4e5d-47fe-9605-bb02c2c94212" },
                    { 9, "Permission", "Read", "34b824a6-8bca-4760-b2cb-628224507b98" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Email",
                table: "AspNetUsers");

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21fca900-4e5d-47fe-9605-bb02c2c94212");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "34b824a6-8bca-4760-b2cb-628224507b98");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6aa04a7-3638-415d-8418-c5782e7333f3");

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
    }
}
