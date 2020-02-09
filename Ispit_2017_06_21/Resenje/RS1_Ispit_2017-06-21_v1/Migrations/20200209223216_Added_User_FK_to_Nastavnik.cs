using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_20170621_v1.Migrations
{
    public partial class Added_User_FK_to_Nastavnik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Nastavnik");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Nastavnik",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Nastavnik_UserId",
                table: "Nastavnik",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Nastavnik_Users_UserId",
                table: "Nastavnik",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Nastavnik_Users_UserId",
                table: "Nastavnik");

            migrationBuilder.DropIndex(
                name: "IX_Nastavnik_UserId",
                table: "Nastavnik");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Nastavnik");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Nastavnik",
                nullable: true);
        }
    }
}
