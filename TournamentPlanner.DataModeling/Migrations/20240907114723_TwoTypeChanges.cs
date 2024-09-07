using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class TwoTypeChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 7, 11, 47, 23, 558, DateTimeKind.Utc).AddTicks(7163), new DateTime(2024, 9, 7, 11, 47, 23, 558, DateTimeKind.Utc).AddTicks(7165) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 7, 11, 47, 23, 558, DateTimeKind.Utc).AddTicks(7170), new DateTime(2024, 9, 7, 11, 47, 23, 558, DateTimeKind.Utc).AddTicks(7170) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 13, 11, 46, 31, 383, DateTimeKind.Utc).AddTicks(6314), new DateTime(2024, 8, 13, 11, 46, 31, 383, DateTimeKind.Utc).AddTicks(6316) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 13, 11, 46, 31, 383, DateTimeKind.Utc).AddTicks(6324), new DateTime(2024, 8, 13, 11, 46, 31, 383, DateTimeKind.Utc).AddTicks(6324) });
        }
    }
}
