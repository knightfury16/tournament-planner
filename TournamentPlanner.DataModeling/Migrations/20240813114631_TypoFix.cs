using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class TypoFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentParticipant_Tournaments_TournamentId",
                table: "TournamentParticipant");

            migrationBuilder.RenameColumn(
                name: "TournamentId",
                table: "TournamentParticipant",
                newName: "TournamentsId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentParticipant_TournamentId",
                table: "TournamentParticipant",
                newName: "IX_TournamentParticipant_TournamentsId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentParticipant_Tournaments_TournamentsId",
                table: "TournamentParticipant",
                column: "TournamentsId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TournamentParticipant_Tournaments_TournamentsId",
                table: "TournamentParticipant");

            migrationBuilder.RenameColumn(
                name: "TournamentsId",
                table: "TournamentParticipant",
                newName: "TournamentId");

            migrationBuilder.RenameIndex(
                name: "IX_TournamentParticipant_TournamentsId",
                table: "TournamentParticipant",
                newName: "IX_TournamentParticipant_TournamentId");

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 10, 51, 36, 647, DateTimeKind.Utc).AddTicks(6236), new DateTime(2024, 8, 9, 10, 51, 36, 647, DateTimeKind.Utc).AddTicks(6237) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 8, 9, 10, 51, 36, 647, DateTimeKind.Utc).AddTicks(6243), new DateTime(2024, 8, 9, 10, 51, 36, 647, DateTimeKind.Utc).AddTicks(6244) });

            migrationBuilder.AddForeignKey(
                name: "FK_TournamentParticipant_Tournaments_TournamentId",
                table: "TournamentParticipant",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
