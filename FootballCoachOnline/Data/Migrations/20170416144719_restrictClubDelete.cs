using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FootballCoachOnline.Data.Migrations
{
    public partial class restrictClubDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CoachId",
                table: "Team",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "CoachId",
                table: "Team",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
