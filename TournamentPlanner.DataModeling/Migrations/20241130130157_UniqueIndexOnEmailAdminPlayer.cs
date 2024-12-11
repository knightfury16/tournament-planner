using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class UniqueIndexOnEmailAdminPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 30, 13, 1, 56, 306, DateTimeKind.Utc).AddTicks(1968), new DateTime(2024, 11, 30, 13, 1, 56, 306, DateTimeKind.Utc).AddTicks(1971) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 11, 30, 13, 1, 56, 306, DateTimeKind.Utc).AddTicks(1981), new DateTime(2024, 11, 30, 13, 1, 56, 306, DateTimeKind.Utc).AddTicks(1981) });

            migrationBuilder.InsertData(
                table: "GameTypes",
                columns: new[] { "Id", "CreatedAt", "Name", "UpdatedAt" },
                values: new object[] { 3, new DateTime(2024, 11, 30, 13, 1, 56, 306, DateTimeKind.Utc).AddTicks(1983), "EightBallPool", new DateTime(2024, 11, 30, 13, 1, 56, 306, DateTimeKind.Utc).AddTicks(1983) });

            migrationBuilder.CreateIndex(
                name: "IX_Players_Email",
                table: "Players",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Admins_Email",
                table: "Admins",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Players_Email",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Admins_Email",
                table: "Admins");

            migrationBuilder.DeleteData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 21, 11, 29, 29, 504, DateTimeKind.Utc).AddTicks(4334), new DateTime(2024, 10, 21, 11, 29, 29, 504, DateTimeKind.Utc).AddTicks(4336) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 10, 21, 11, 29, 29, 504, DateTimeKind.Utc).AddTicks(4347), new DateTime(2024, 10, 21, 11, 29, 29, 504, DateTimeKind.Utc).AddTicks(4347) });
        }
    }
}
