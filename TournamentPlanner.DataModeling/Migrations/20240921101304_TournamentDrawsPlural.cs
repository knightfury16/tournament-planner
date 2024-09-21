using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class TournamentDrawsPlural : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 21, 10, 13, 4, 221, DateTimeKind.Utc).AddTicks(1147), new DateTime(2024, 9, 21, 10, 13, 4, 221, DateTimeKind.Utc).AddTicks(1149) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 21, 10, 13, 4, 221, DateTimeKind.Utc).AddTicks(1154), new DateTime(2024, 9, 21, 10, 13, 4, 221, DateTimeKind.Utc).AddTicks(1155) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 13, 12, 38, 39, 345, DateTimeKind.Utc).AddTicks(9569), new DateTime(2024, 9, 13, 12, 38, 39, 345, DateTimeKind.Utc).AddTicks(9571) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 13, 12, 38, 39, 345, DateTimeKind.Utc).AddTicks(9576), new DateTime(2024, 9, 13, 12, 38, 39, 345, DateTimeKind.Utc).AddTicks(9576) });
        }
    }
}
