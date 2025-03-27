using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TournamentPlanner.DataModeling.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameStatisticEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "60ac9dde-deab-43a3-a6a6-912409386fb5"
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a20a3c80-b958-4187-b8ac-e56d510b0c21"
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5284c0e-99f1-4bf1-90f6-4ae22c4125dd"
            );

            migrationBuilder.CreateTable(
                name: "GameStatistics",
                columns: table => new
                {
                    Id = table
                        .Column<int>(type: "integer", nullable: false)
                        .Annotation(
                            "Npgsql:ValueGenerationStrategy",
                            NpgsqlValueGenerationStrategy.IdentityByDefaultColumn
                        ),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    GameTypeId = table.Column<int>(type: "integer", nullable: false),
                    GamePlayed = table.Column<int>(type: "integer", nullable: false),
                    GameWon = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                    UpdatedAt = table.Column<DateTime>(
                        type: "timestamp with time zone",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStatistics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameStatistics_GameTypes_GameTypeId",
                        column: x => x.GameTypeId,
                        principalTable: "GameTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                    table.ForeignKey(
                        name: "FK_GameStatistics_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                }
            );

            // Transfer data - assuming GameTypeId 1 (table tennis) is the default game type
            // Custom migration code want to preserve the data
            migrationBuilder.Sql(
                @"
                    INSERT INTO ""GameStatistics"" (""PlayerId"", ""GameTypeId"", ""GamePlayed"", ""GameWon"", ""CreatedAt"", ""UpdatedAt"")
                    SELECT 
                        ""Id"" as ""PlayerId"", 
                        1 as ""GameTypeId"", -- Default GameTypeId, adjust as needed
                        ""GamePlayed"", 
                        ""GameWon"",
                        NOW() as ""CreatedAt"",
                        NOW() as ""UpdatedAt""
                    FROM ""Players""
                    WHERE ""GamePlayed"" > 0 OR ""GameWon"" > 0
                "
            );

            migrationBuilder.DropColumn(name: "GamePlayed", table: "Players");

            migrationBuilder.DropColumn(name: "GameWon", table: "Players");

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "c5fed5ac-0c0c-4369-9b9d-00d950537ede"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "c5fed5ac-0c0c-4369-9b9d-00d950537ede"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "c5fed5ac-0c0c-4369-9b9d-00d950537ede"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "9942fa7a-2089-455a-a3e9-2c869320d261"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "6de344e7-e4ca-4ab0-bf15-1148aaa238e3"
            );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6de344e7-e4ca-4ab0-bf15-1148aaa238e3", null, "Player", "PLAYER" },
                    { "9942fa7a-2089-455a-a3e9-2c869320d261", null, "Admin", "ADMIN" },
                    { "c5fed5ac-0c0c-4369-9b9d-00d950537ede", null, "Moderator", "MODERATOR" },
                }
            );

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[]
                {
                    new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7600),
                    new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7600),
                }
            );

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[]
                {
                    new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7610),
                    new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7610),
                }
            );

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[]
                {
                    new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7610),
                    new DateTime(2025, 3, 23, 9, 46, 22, 829, DateTimeKind.Utc).AddTicks(7610),
                }
            );

            migrationBuilder.CreateIndex(
                name: "IX_GameStatistics_GameTypeId",
                table: "GameStatistics",
                column: "GameTypeId"
            );

            migrationBuilder.CreateIndex(
                name: "IX_GameStatistics_PlayerId",
                table: "GameStatistics",
                column: "PlayerId"
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "GameStatistics");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6de344e7-e4ca-4ab0-bf15-1148aaa238e3"
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9942fa7a-2089-455a-a3e9-2c869320d261"
            );

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c5fed5ac-0c0c-4369-9b9d-00d950537ede"
            );

            migrationBuilder.AddColumn<int>(
                name: "GamePlayed",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.AddColumn<int>(
                name: "GameWon",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 0
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1,
                column: "RoleId",
                value: "c5284c0e-99f1-4bf1-90f6-4ae22c4125dd"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2,
                column: "RoleId",
                value: "c5284c0e-99f1-4bf1-90f6-4ae22c4125dd"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3,
                column: "RoleId",
                value: "c5284c0e-99f1-4bf1-90f6-4ae22c4125dd"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4,
                column: "RoleId",
                value: "60ac9dde-deab-43a3-a6a6-912409386fb5"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5,
                column: "RoleId",
                value: "60ac9dde-deab-43a3-a6a6-912409386fb5"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6,
                column: "RoleId",
                value: "60ac9dde-deab-43a3-a6a6-912409386fb5"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7,
                column: "RoleId",
                value: "60ac9dde-deab-43a3-a6a6-912409386fb5"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8,
                column: "RoleId",
                value: "60ac9dde-deab-43a3-a6a6-912409386fb5"
            );

            migrationBuilder.UpdateData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9,
                column: "RoleId",
                value: "a20a3c80-b958-4187-b8ac-e56d510b0c21"
            );

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "60ac9dde-deab-43a3-a6a6-912409386fb5", null, "Admin", "ADMIN" },
                    { "a20a3c80-b958-4187-b8ac-e56d510b0c21", null, "Player", "PLAYER" },
                    { "c5284c0e-99f1-4bf1-90f6-4ae22c4125dd", null, "Moderator", "MODERATOR" },
                }
            );

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[]
                {
                    new DateTime(2024, 12, 2, 9, 23, 34, 84, DateTimeKind.Utc).AddTicks(5450),
                    new DateTime(2024, 12, 2, 9, 23, 34, 84, DateTimeKind.Utc).AddTicks(5455),
                }
            );

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[]
                {
                    new DateTime(2024, 12, 2, 9, 23, 34, 84, DateTimeKind.Utc).AddTicks(5463),
                    new DateTime(2024, 12, 2, 9, 23, 34, 84, DateTimeKind.Utc).AddTicks(5464),
                }
            );

            migrationBuilder.UpdateData(
                table: "GameTypes",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "UpdatedAt" },
                values: new object[]
                {
                    new DateTime(2024, 12, 2, 9, 23, 34, 84, DateTimeKind.Utc).AddTicks(5465),
                    new DateTime(2024, 12, 2, 9, 23, 34, 84, DateTimeKind.Utc).AddTicks(5465),
                }
            );
        }
    }
}
