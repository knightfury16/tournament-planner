using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class AddedPropertyInMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CourtName",
                table: "Matches",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Matches",
                type: "interval",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 8, 16, 50, 14, 280, DateTimeKind.Utc).AddTicks(9322), new DateTime(2024, 9, 8, 16, 50, 14, 280, DateTimeKind.Utc).AddTicks(9324) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 8, 16, 50, 14, 280, DateTimeKind.Utc).AddTicks(9333), new DateTime(2024, 9, 8, 16, 50, 14, 280, DateTimeKind.Utc).AddTicks(9333) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourtName",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Matches");

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 7, 14, 23, 8, 421, DateTimeKind.Utc).AddTicks(8604), new DateTime(2024, 9, 7, 14, 23, 8, 421, DateTimeKind.Utc).AddTicks(8605) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 7, 14, 23, 8, 421, DateTimeKind.Utc).AddTicks(8610), new DateTime(2024, 9, 7, 14, 23, 8, 421, DateTimeKind.Utc).AddTicks(8610) });
        }
    }
}
