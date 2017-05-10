using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballCoachOnline.Data.Migrations
{
    public partial class TeamId_MatchStats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamId",
                table: "MatchStats",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MatchStats_TeamId",
                table: "MatchStats",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchStats_Team_TeamId",
                table: "MatchStats",
                column: "TeamId",
                principalTable: "Team",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchStats_Team_TeamId",
                table: "MatchStats");

            migrationBuilder.DropIndex(
                name: "IX_MatchStats_TeamId",
                table: "MatchStats");

            migrationBuilder.DropColumn(
                name: "TeamId",
                table: "MatchStats");
        }
    }
}
