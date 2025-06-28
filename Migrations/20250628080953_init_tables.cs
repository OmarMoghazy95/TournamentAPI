using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tournament.Api.Migrations
{
    /// <inheritdoc />
    public partial class init_tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tournament",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    TeamCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tournament", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TournamentTeam",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamName = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentTeam", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentTeam_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TournamentMatch",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TournamentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TeamAId = table.Column<Guid>(type: "TEXT", nullable: true),
                    TeamBId = table.Column<Guid>(type: "TEXT", nullable: true),
                    WinnerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Status = table.Column<byte>(type: "INTEGER", nullable: false),
                    RoundNumber = table.Column<uint>(type: "INTEGER", nullable: false),
                    MatchAId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MatchBId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TournamentMatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TournamentMatch_TournamentMatch_MatchAId",
                        column: x => x.MatchAId,
                        principalTable: "TournamentMatch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TournamentMatch_TournamentMatch_MatchBId",
                        column: x => x.MatchBId,
                        principalTable: "TournamentMatch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TournamentMatch_TournamentTeam_TeamAId",
                        column: x => x.TeamAId,
                        principalTable: "TournamentTeam",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TournamentMatch_TournamentTeam_TeamBId",
                        column: x => x.TeamBId,
                        principalTable: "TournamentTeam",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TournamentMatch_TournamentTeam_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "TournamentTeam",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TournamentMatch_Tournament_TournamentId",
                        column: x => x.TournamentId,
                        principalTable: "Tournament",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatch_MatchAId",
                table: "TournamentMatch",
                column: "MatchAId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatch_MatchBId",
                table: "TournamentMatch",
                column: "MatchBId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatch_TeamAId",
                table: "TournamentMatch",
                column: "TeamAId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatch_TeamBId",
                table: "TournamentMatch",
                column: "TeamBId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatch_TournamentId",
                table: "TournamentMatch",
                column: "TournamentId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentMatch_WinnerId",
                table: "TournamentMatch",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_TournamentTeam_TournamentId",
                table: "TournamentTeam",
                column: "TournamentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TournamentMatch");

            migrationBuilder.DropTable(
                name: "TournamentTeam");

            migrationBuilder.DropTable(
                name: "Tournament");
        }
    }
}
