using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballCoachOnline.Data.Migrations
{
    public partial class playerStatsCompetition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompetitionId",
                table: "PlayerStats",
                nullable: false,
                defaultValue: 4);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerStats_CompetitionId",
                table: "PlayerStats",
                column: "CompetitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerStats_Competition_CompetitionId",
                table: "PlayerStats",
                column: "CompetitionId",
                principalTable: "Competition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerStats_Competition_CompetitionId",
                table: "PlayerStats");

            migrationBuilder.DropIndex(
                name: "IX_PlayerStats_CompetitionId",
                table: "PlayerStats");

            migrationBuilder.DropColumn(
                name: "CompetitionId",
                table: "PlayerStats");
        }
    }
}
