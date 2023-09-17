using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.TMEF.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coaches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coaches", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Opponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PlayingAddress_Street = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayingAddress_Number = table.Column<int>(type: "int", nullable: true),
                    PlayingAddress_Extension = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayingAddress_City = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Opponents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamHeadCoachId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teams_Coaches_TeamHeadCoachId",
                        column: x => x.TeamHeadCoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlayerNumber = table.Column<int>(type: "int", nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Players_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CareTakers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EMailAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<int>(type: "int", nullable: false),
                    HasCar = table.Column<bool>(type: "bit", nullable: false),
                    PlayerId = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CareTakers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareTakers_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsHomeGame = table.Column<bool>(type: "bit", nullable: false),
                    CoachId = table.Column<int>(type: "int", nullable: true),
                    LaundryDutyId = table.Column<int>(type: "int", nullable: true),
                    OpponentId = table.Column<int>(type: "int", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_CareTakers_LaundryDutyId",
                        column: x => x.LaundryDutyId,
                        principalTable: "CareTakers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Coaches_CoachId",
                        column: x => x.CoachId,
                        principalTable: "Coaches",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Opponents_OpponentId",
                        column: x => x.OpponentId,
                        principalTable: "Opponents",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Games_Teams_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Teams",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GamePlayer",
                columns: table => new
                {
                    GamesId = table.Column<int>(type: "int", nullable: false),
                    PlayersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayer", x => new { x.GamesId, x.PlayersId });
                    table.ForeignKey(
                        name: "FK_GamePlayer_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlayer_Players_PlayersId",
                        column: x => x.PlayersId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Coaches",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Tim" },
                    { 2, "Iris" }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Name", "TeamHeadCoachId" },
                values: new object[] { 1, "VU16", null });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "EmailAddress", "Name", "PlayerNumber", "TeamId" },
                values: new object[,]
                {
                    { 1, null, "Agnes", 10, 1 },
                    { 2, null, "Linda", 2, 1 },
                    { 3, null, "Debbie", 3, 1 },
                    { 4, null, "Sena", 4, 1 }
                });

            migrationBuilder.InsertData(
                table: "CareTakers",
                columns: new[] { "Id", "EMailAddress", "GameId", "HasCar", "Name", "PhoneNumber", "PlayerId" },
                values: new object[,]
                {
                    { 1, null, null, true, "Johan", 0, 1 },
                    { 2, null, null, true, "Manuela", 0, 1 },
                    { 3, null, null, false, "Inge", 0, 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CareTakers_GameId",
                table: "CareTakers",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_CareTakers_PlayerId",
                table: "CareTakers",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayer_PlayersId",
                table: "GamePlayer",
                column: "PlayersId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_CoachId",
                table: "Games",
                column: "CoachId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_LaundryDutyId",
                table: "Games",
                column: "LaundryDutyId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_OpponentId",
                table: "Games",
                column: "OpponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_TeamId",
                table: "Games",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Opponents_Name",
                table: "Opponents",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Players_TeamId",
                table: "Players",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Teams_TeamHeadCoachId",
                table: "Teams",
                column: "TeamHeadCoachId");

            migrationBuilder.AddForeignKey(
                name: "FK_CareTakers_Games_GameId",
                table: "CareTakers",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CareTakers_Games_GameId",
                table: "CareTakers");

            migrationBuilder.DropTable(
                name: "GamePlayer");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "CareTakers");

            migrationBuilder.DropTable(
                name: "Opponents");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Coaches");
        }
    }
}
