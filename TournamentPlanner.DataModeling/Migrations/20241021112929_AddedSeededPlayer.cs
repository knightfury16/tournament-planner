using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class AddedSeededPlayer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SeededPlayers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    MatchTypeId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeededPlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeededPlayers_MatchTypes_MatchTypeId",
                        column: x => x.MatchTypeId,
                        principalTable: "MatchTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeededPlayers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_SeededPlayers_MatchTypeId",
                table: "SeededPlayers",
                column: "MatchTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SeededPlayers_PlayerId",
                table: "SeededPlayers",
                column: "PlayerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SeededPlayers");

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
    }
}
