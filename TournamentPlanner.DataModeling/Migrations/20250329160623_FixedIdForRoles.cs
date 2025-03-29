using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class FixedIdForRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6de344e7-e4ca-4ab0-bf15-1148aaa238e3");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9942fa7a-2089-455a-a3e9-2c869320d261");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5fed5ac-0c0c-4369-9b9d-00d950537ede");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "7c8b1c8b-d610-438b-a45d-9034a dbab321");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "7c8b1c8b-d610-438b-a45d-9034a dbab321");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "7c8b1c8b-d610-438b-a45d-9034a dbab321");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "1ee445a5-b34c-4e99-b605-72bac92b95ec");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "1ee445a5-b34c-4e99-b605-72bac92b95ec");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "1ee445a5-b34c-4e99-b605-72bac92b95ec");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "1ee445a5-b34c-4e99-b605-72bac92b95ec");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "1ee445a5-b34c-4e99-b605-72bac92b95ec");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "2e3a7d1e-1b9c-4a9c-b02e-2e910a34c26f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1ee445a5-b34c-4e99-b605-72bac92b95ec", "dbef7fdb-3ffe-4673-b22b-7ba363ae4eb7", "Admin", "ADMIN" },
                    { "2e3a7d1e-1b9c-4a9c-b02e-2e910a34c26f", "060ba2ba-b5c8-4c51-8a07-ed8045a0adcc", "Player", "PLAYER" },
                    { "7c8b1c8b-d610-438b-a45d-9034a dbab321", "6547d0aa-0835-45b3-9344-85809e0b2795", "Moderator", "MODERATOR" }
                });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 29, 16, 6, 23, 290, DateTimeKind.Utc).AddTicks(2340), new DateTime(2025, 3, 29, 16, 6, 23, 290, DateTimeKind.Utc).AddTicks(2340) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 29, 16, 6, 23, 290, DateTimeKind.Utc).AddTicks(2350), new DateTime(2025, 3, 29, 16, 6, 23, 290, DateTimeKind.Utc).AddTicks(2350) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 29, 16, 6, 23, 290, DateTimeKind.Utc).AddTicks(2350), new DateTime(2025, 3, 29, 16, 6, 23, 290, DateTimeKind.Utc).AddTicks(2350) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ee445a5-b34c-4e99-b605-72bac92b95ec");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e3a7d1e-1b9c-4a9c-b02e-2e910a34c26f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c8b1c8b-d610-438b-a45d-9034a dbab321");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "c5fed5ac-0c0c-4369-9b9d-00d950537ede");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "c5fed5ac-0c0c-4369-9b9d-00d950537ede");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "c5fed5ac-0c0c-4369-9b9d-00d950537ede");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "6de344e7-e4ca-4ab0-bf15-1148aaa238e3");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6de344e7-e4ca-4ab0-bf15-1148aaa238e3", null, "Player", "PLAYER" },
                    { "9942fa7a-2089-455a-a3e9-2c869320d261", null, "Admin", "ADMIN" },
                    { "c5fed5ac-0c0c-4369-9b9d-00d950537ede", null, "Moderator", "MODERATOR" }
                });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7600), new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7600) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7610), new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7610) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7610), new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7610) });
        }
    }
}
