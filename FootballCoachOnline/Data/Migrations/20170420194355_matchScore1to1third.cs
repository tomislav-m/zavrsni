using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballCoachOnline.Data.Migrations
{
    public partial class matchScore1to1third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchScore_Match_MatchId",
                table: "MatchScore");

            migrationBuilder.DropIndex(
                name: "IX_MatchScore_MatchId",
                table: "MatchScore");

            migrationBuilder.DropColumn(
                name: "MatchId",
                table: "MatchScore");

            migrationBuilder.AddColumn<int>(
                name: "MatchScoreId",
                table: "Match",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Match_MatchScoreId",
                table: "Match",
                column: "MatchScoreId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_MatchScore_MatchScoreId",
                table: "Match",
                column: "MatchScoreId",
                principalTable: "MatchScore",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Match_MatchScore_MatchScoreId",
                table: "Match");

            migrationBuilder.DropIndex(
                name: "IX_Match_MatchScoreId",
                table: "Match");

            migrationBuilder.DropColumn(
                name: "MatchScoreId",
                table: "Match");

            migrationBuilder.AddColumn<int>(
                name: "MatchId",
                table: "MatchScore",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MatchScore_MatchId",
                table: "MatchScore",
                column: "MatchId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchScore_Match_MatchId",
                table: "MatchScore",
                column: "MatchId",
                principalTable: "Match",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
