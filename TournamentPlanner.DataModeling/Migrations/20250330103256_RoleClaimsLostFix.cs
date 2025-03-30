using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class RoleClaimsLostFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 10, "Permission", "Read", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 11, "Permission", "Edit", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 12, "Permission", "Create", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 13, "Permission", "Delete", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 14, "Permission", "AddScore", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 15, "Permission", "Read", "7c8b1c8b-d610-438b-a45d-9034a dbab321" },
                    { 16, "Permission", "Edit", "7c8b1c8b-d610-438b-a45d-9034a dbab321" },
                    { 17, "Permission", "AddScore", "7c8b1c8b-d610-438b-a45d-9034a dbab321" },
                    { 18, "Permission", "Read", "2e3a7d1e-1b9c-4a9c-b02e-2e910a34c26f" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ee445a5-b34c-4e99-b605-72bac92b95ec",
                column: "ConcurrencyStamp",
                value: "56fc31bb-b104-4a8f-ac9e-5a03becc8991");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e3a7d1e-1b9c-4a9c-b02e-2e910a34c26f",
                column: "ConcurrencyStamp",
                value: "2a0aa940-50d8-4775-b372-1bfee78250ab");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c8b1c8b-d610-438b-a45d-9034a dbab321",
                column: "ConcurrencyStamp",
                value: "9f5f2043-7d6d-40e2-9a71-aced7c99c212");

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 30, 10, 32, 56, 141, DateTimeKind.Utc).AddTicks(8250), new DateTime(2025, 3, 30, 10, 32, 56, 141, DateTimeKind.Utc).AddTicks(8250) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 30, 10, 32, 56, 141, DateTimeKind.Utc).AddTicks(8260), new DateTime(2025, 3, 30, 10, 32, 56, 141, DateTimeKind.Utc).AddTicks(8260) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2025, 3, 30, 10, 32, 56, 141, DateTimeKind.Utc).AddTicks(8260), new DateTime(2025, 3, 30, 10, 32, 56, 141, DateTimeKind.Utc).AddTicks(8260) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permission", "Read", "7c8b1c8b-d610-438b-a45d-9034a dbab321" },
                    { 2, "Permission", "Edit", "7c8b1c8b-d610-438b-a45d-9034a dbab321" },
                    { 3, "Permission", "AddScore", "7c8b1c8b-d610-438b-a45d-9034a dbab321" },
                    { 4, "Permission", "Read", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 5, "Permission", "Edit", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 6, "Permission", "Create", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 7, "Permission", "Delete", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 8, "Permission", "AddScore", "1ee445a5-b34c-4e99-b605-72bac92b95ec" },
                    { 9, "Permission", "Read", "2e3a7d1e-1b9c-4a9c-b02e-2e910a34c26f" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1ee445a5-b34c-4e99-b605-72bac92b95ec",
                column: "ConcurrencyStamp",
                value: "dbef7fdb-3ffe-4673-b22b-7ba363ae4eb7");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e3a7d1e-1b9c-4a9c-b02e-2e910a34c26f",
                column: "ConcurrencyStamp",
                value: "060ba2ba-b5c8-4c51-8a07-ed8045a0adcc");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7c8b1c8b-d610-438b-a45d-9034a dbab321",
                column: "ConcurrencyStamp",
                value: "6547d0aa-0835-45b3-9344-85809e0b2795");

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
    }
}
