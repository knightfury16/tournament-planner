using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class AddedCurrentStateProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentState",
                table: "Tournaments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 30, 17, 26, 9, 61, DateTimeKind.Utc).AddTicks(6275), new DateTime(2024, 9, 30, 17, 26, 9, 61, DateTimeKind.Utc).AddTicks(6277) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 30, 17, 26, 9, 61, DateTimeKind.Utc).AddTicks(6284), new DateTime(2024, 9, 30, 17, 26, 9, 61, DateTimeKind.Utc).AddTicks(6284) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentState",
                table: "Tournaments");

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
    }
}
