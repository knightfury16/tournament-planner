using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class ReconfiguredRoundWithMatchType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_MatchTypes_MatchTypeId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "Round",
                table: "MatchTypes");

            migrationBuilder.RenameColumn(
                name: "MatchTypeId",
                table: "Matches",
                newName: "RoundId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_MatchTypeId",
                table: "Matches",
                newName: "IX_Matches_RoundId");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompleted",
                table: "MatchTypes",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Rounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RoundName = table.Column<string>(type: "text", nullable: false),
                    RoundNumber = table.Column<int>(type: "integer", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MatchTypeId = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rounds_MatchTypes_MatchTypeId",
                        column: x => x.MatchTypeId,
                        principalTable: "MatchTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 11, 17, 45, 0, 299, DateTimeKind.Utc).AddTicks(6070), new DateTime(2024, 9, 11, 17, 45, 0, 299, DateTimeKind.Utc).AddTicks(6071) });

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[] { new DateTime(2024, 9, 11, 17, 45, 0, 299, DateTimeKind.Utc).AddTicks(6076), new DateTime(2024, 9, 11, 17, 45, 0, 299, DateTimeKind.Utc).AddTicks(6076) });

            migrationBuilder.CreateIndex(
                name: "IX_Rounds_MatchTypeId",
                table: "Rounds",
                column: "MatchTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Rounds_RoundId",
                table: "Matches",
                column: "RoundId",
                principalTable: "Rounds",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Rounds_RoundId",
                table: "Matches");

            migrationBuilder.DropTable(
                name: "Rounds");

            migrationBuilder.DropColumn(
                name: "IsCompleted",
                table: "MatchTypes");

            migrationBuilder.RenameColumn(
                name: "RoundId",
                table: "Matches",
                newName: "MatchTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_RoundId",
                table: "Matches",
                newName: "IX_Matches_MatchTypeId");

            migrationBuilder.AddColumn<int>(
                name: "Round",
                table: "MatchTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_MatchTypes_MatchTypeId",
                table: "Matches",
                column: "MatchTypeId",
                principalTable: "MatchTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
