using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class AddedRoundInMatchType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Round",
                table: "MatchTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Round",
                table: "MatchTypes",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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
    }
}
