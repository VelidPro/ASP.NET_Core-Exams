using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class Added_MaturskiIspit_MaturskiIspitStavke_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspit_Nastavnik_NastavnikId",
                table: "MaturskiIspit");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspit_Predmet_PredmetId",
                table: "MaturskiIspit");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspit_Skola_SkolaId",
                table: "MaturskiIspit");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspitStavka_MaturskiIspit_MaturskiIspitId",
                table: "MaturskiIspitStavka");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspitStavka_Ucenik_UcenikId",
                table: "MaturskiIspitStavka");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaturskiIspitStavka",
                table: "MaturskiIspitStavka");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaturskiIspit",
                table: "MaturskiIspit");

            migrationBuilder.RenameTable(
                name: "MaturskiIspitStavka",
                newName: "MaturskiIspitStavke");

            migrationBuilder.RenameTable(
                name: "MaturskiIspit",
                newName: "MaturskiIspiti");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspitStavka_UcenikId",
                table: "MaturskiIspitStavke",
                newName: "IX_MaturskiIspitStavke_UcenikId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspitStavka_MaturskiIspitId",
                table: "MaturskiIspitStavke",
                newName: "IX_MaturskiIspitStavke_MaturskiIspitId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspit_SkolaId",
                table: "MaturskiIspiti",
                newName: "IX_MaturskiIspiti_SkolaId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspit_PredmetId",
                table: "MaturskiIspiti",
                newName: "IX_MaturskiIspiti_PredmetId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspit_NastavnikId",
                table: "MaturskiIspiti",
                newName: "IX_MaturskiIspiti_NastavnikId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaturskiIspitStavke",
                table: "MaturskiIspitStavke",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaturskiIspiti",
                table: "MaturskiIspiti",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspiti_Nastavnik_NastavnikId",
                table: "MaturskiIspiti",
                column: "NastavnikId",
                principalTable: "Nastavnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspiti_Predmet_PredmetId",
                table: "MaturskiIspiti",
                column: "PredmetId",
                principalTable: "Predmet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspiti_Skola_SkolaId",
                table: "MaturskiIspiti",
                column: "SkolaId",
                principalTable: "Skola",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspitStavke_MaturskiIspiti_MaturskiIspitId",
                table: "MaturskiIspitStavke",
                column: "MaturskiIspitId",
                principalTable: "MaturskiIspiti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspitStavke_Ucenik_UcenikId",
                table: "MaturskiIspitStavke",
                column: "UcenikId",
                principalTable: "Ucenik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspiti_Nastavnik_NastavnikId",
                table: "MaturskiIspiti");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspiti_Predmet_PredmetId",
                table: "MaturskiIspiti");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspiti_Skola_SkolaId",
                table: "MaturskiIspiti");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspitStavke_MaturskiIspiti_MaturskiIspitId",
                table: "MaturskiIspitStavke");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspitStavke_Ucenik_UcenikId",
                table: "MaturskiIspitStavke");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaturskiIspitStavke",
                table: "MaturskiIspitStavke");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MaturskiIspiti",
                table: "MaturskiIspiti");

            migrationBuilder.RenameTable(
                name: "MaturskiIspitStavke",
                newName: "MaturskiIspitStavka");

            migrationBuilder.RenameTable(
                name: "MaturskiIspiti",
                newName: "MaturskiIspit");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspitStavke_UcenikId",
                table: "MaturskiIspitStavka",
                newName: "IX_MaturskiIspitStavka_UcenikId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspitStavke_MaturskiIspitId",
                table: "MaturskiIspitStavka",
                newName: "IX_MaturskiIspitStavka_MaturskiIspitId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspiti_SkolaId",
                table: "MaturskiIspit",
                newName: "IX_MaturskiIspit_SkolaId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspiti_PredmetId",
                table: "MaturskiIspit",
                newName: "IX_MaturskiIspit_PredmetId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspiti_NastavnikId",
                table: "MaturskiIspit",
                newName: "IX_MaturskiIspit_NastavnikId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaturskiIspitStavka",
                table: "MaturskiIspitStavka",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MaturskiIspit",
                table: "MaturskiIspit",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspit_Nastavnik_NastavnikId",
                table: "MaturskiIspit",
                column: "NastavnikId",
                principalTable: "Nastavnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspit_Predmet_PredmetId",
                table: "MaturskiIspit",
                column: "PredmetId",
                principalTable: "Predmet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspit_Skola_SkolaId",
                table: "MaturskiIspit",
                column: "SkolaId",
                principalTable: "Skola",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspitStavka_MaturskiIspit_MaturskiIspitId",
                table: "MaturskiIspitStavka",
                column: "MaturskiIspitId",
                principalTable: "MaturskiIspit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspitStavka_Ucenik_UcenikId",
                table: "MaturskiIspitStavka",
                column: "UcenikId",
                principalTable: "Ucenik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
