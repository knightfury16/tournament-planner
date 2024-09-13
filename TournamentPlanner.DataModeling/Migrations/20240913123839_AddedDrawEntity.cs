using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class AddedDrawEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchTypes_Tournaments_TournamentId",
                table: "MatchTypes");

            migrationBuilder.DropIndex(
                name: "IX_MatchTypes_TournamentId",
                table: "MatchTypes");

            migrationBuilder.DropColumn(
                name: "TournamentId",
                table: "MatchTypes");

            migrationBuilder.CreateTable(
                name: "Draws",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TournamentId = table.Column<int>(type: "integer", nullable: false),
                    MatchTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Draws", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Draws_MatchTypes_MatchTypeId",
                        column: x => x.MatchTypeId,
                        principalTable: "MatchTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Draws_Tournaments_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Draws_MatchTypeId",
                table: "Draws",
                column: "MatchTypeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Draws_TournamentId",
                table: "Draws",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Draws");

            migrationBuilder.AddColumn<int>(
                name: "TournamentId",
                table: "MatchTypes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_MatchTypes_TournamentId",
                table: "MatchTypes",
                column: "TournamentId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchTypes_Tournaments_TournamentId",
                table: "MatchTypes",
                column: "TournamentId",
                principalTable: "Tournaments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
