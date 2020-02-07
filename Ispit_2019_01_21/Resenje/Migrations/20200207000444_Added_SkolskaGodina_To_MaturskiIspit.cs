using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class Added_SkolskaGodina_To_MaturskiIspit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SkolskaGodinaId",
                table: "MaturskiIspiti",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_MaturskiIspiti_SkolskaGodinaId",
                table: "MaturskiIspiti",
                column: "SkolskaGodinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspiti_SkolskaGodina_SkolskaGodinaId",
                table: "MaturskiIspiti",
                column: "SkolskaGodinaId",
                principalTable: "SkolskaGodina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspiti_SkolskaGodina_SkolskaGodinaId",
                table: "MaturskiIspiti");

            migrationBuilder.DropIndex(
                name: "IX_MaturskiIspiti_SkolskaGodinaId",
                table: "MaturskiIspiti");

            migrationBuilder.DropColumn(
                name: "SkolskaGodinaId",
                table: "MaturskiIspiti");
        }
    }
}
