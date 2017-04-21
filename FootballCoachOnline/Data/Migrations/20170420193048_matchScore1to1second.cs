using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballCoachOnline.Data.Migrations
{
    public partial class matchScore1to1second : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MatchScore_MatchId",
                table: "MatchScore");

            migrationBuilder.DropColumn(
                name: "MatchScore",
                table: "Match");

            migrationBuilder.CreateIndex(
                name: "IX_MatchScore_MatchId",
                table: "MatchScore",
                column: "MatchId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_MatchScore_MatchId",
                table: "MatchScore");

            migrationBuilder.AddColumn<int>(
                name: "MatchScore",
                table: "Match",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MatchScore_MatchId",
                table: "MatchScore",
                column: "MatchId");
        }
    }
}
