using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class Fix_Names : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "FK_MaturskiIspiti_SkolskaGodina_SkolskaGodinaId",
                table: "MaturskiIspiti");

            migrationBuilder.DropForeignKey(
                name: "FK_MaturskiIspitStavke_MaturskiIspiti_PopravniIspitId",
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
                newName: "PopravniIspitStavke");

            migrationBuilder.RenameTable(
                name: "MaturskiIspiti",
                newName: "PopravniIspiti");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspitStavke_UcenikId",
                table: "PopravniIspitStavke",
                newName: "IX_PopravniIspitStavke_UcenikId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspitStavke_PopravniIspitId",
                table: "PopravniIspitStavke",
                newName: "IX_PopravniIspitStavke_PopravniIspitId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspiti_SkolskaGodinaId",
                table: "PopravniIspiti",
                newName: "IX_PopravniIspiti_SkolskaGodinaId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspiti_SkolaId",
                table: "PopravniIspiti",
                newName: "IX_PopravniIspiti_SkolaId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspiti_PredmetId",
                table: "PopravniIspiti",
                newName: "IX_PopravniIspiti_PredmetId");

            migrationBuilder.RenameIndex(
                name: "IX_MaturskiIspiti_NastavnikId",
                table: "PopravniIspiti",
                newName: "IX_PopravniIspiti_NastavnikId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PopravniIspitStavke",
                table: "PopravniIspitStavke",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PopravniIspiti",
                table: "PopravniIspiti",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PopravniIspiti_Nastavnik_NastavnikId",
                table: "PopravniIspiti",
                column: "NastavnikId",
                principalTable: "Nastavnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PopravniIspiti_Predmet_PredmetId",
                table: "PopravniIspiti",
                column: "PredmetId",
                principalTable: "Predmet",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PopravniIspiti_Skola_SkolaId",
                table: "PopravniIspiti",
                column: "SkolaId",
                principalTable: "Skola",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PopravniIspiti_SkolskaGodina_SkolskaGodinaId",
                table: "PopravniIspiti",
                column: "SkolskaGodinaId",
                principalTable: "SkolskaGodina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PopravniIspitStavke_PopravniIspiti_PopravniIspitId",
                table: "PopravniIspitStavke",
                column: "PopravniIspitId",
                principalTable: "PopravniIspiti",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PopravniIspitStavke_Ucenik_UcenikId",
                table: "PopravniIspitStavke",
                column: "UcenikId",
                principalTable: "Ucenik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PopravniIspiti_Nastavnik_NastavnikId",
                table: "PopravniIspiti");

            migrationBuilder.DropForeignKey(
                name: "FK_PopravniIspiti_Predmet_PredmetId",
                table: "PopravniIspiti");

            migrationBuilder.DropForeignKey(
                name: "FK_PopravniIspiti_Skola_SkolaId",
                table: "PopravniIspiti");

            migrationBuilder.DropForeignKey(
                name: "FK_PopravniIspiti_SkolskaGodina_SkolskaGodinaId",
                table: "PopravniIspiti");

            migrationBuilder.DropForeignKey(
                name: "FK_PopravniIspitStavke_PopravniIspiti_PopravniIspitId",
                table: "PopravniIspitStavke");

            migrationBuilder.DropForeignKey(
                name: "FK_PopravniIspitStavke_Ucenik_UcenikId",
                table: "PopravniIspitStavke");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PopravniIspitStavke",
                table: "PopravniIspitStavke");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PopravniIspiti",
                table: "PopravniIspiti");

            migrationBuilder.RenameTable(
                name: "PopravniIspitStavke",
                newName: "MaturskiIspitStavke");

            migrationBuilder.RenameTable(
                name: "PopravniIspiti",
                newName: "MaturskiIspiti");

            migrationBuilder.RenameIndex(
                name: "IX_PopravniIspitStavke_UcenikId",
                table: "MaturskiIspitStavke",
                newName: "IX_MaturskiIspitStavke_UcenikId");

            migrationBuilder.RenameIndex(
                name: "IX_PopravniIspitStavke_PopravniIspitId",
                table: "MaturskiIspitStavke",
                newName: "IX_MaturskiIspitStavke_PopravniIspitId");

            migrationBuilder.RenameIndex(
                name: "IX_PopravniIspiti_SkolskaGodinaId",
                table: "MaturskiIspiti",
                newName: "IX_MaturskiIspiti_SkolskaGodinaId");

            migrationBuilder.RenameIndex(
                name: "IX_PopravniIspiti_SkolaId",
                table: "MaturskiIspiti",
                newName: "IX_MaturskiIspiti_SkolaId");

            migrationBuilder.RenameIndex(
                name: "IX_PopravniIspiti_PredmetId",
                table: "MaturskiIspiti",
                newName: "IX_MaturskiIspiti_PredmetId");

            migrationBuilder.RenameIndex(
                name: "IX_PopravniIspiti_NastavnikId",
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
                name: "FK_MaturskiIspiti_SkolskaGodina_SkolskaGodinaId",
                table: "MaturskiIspiti",
                column: "SkolskaGodinaId",
                principalTable: "SkolskaGodina",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MaturskiIspitStavke_MaturskiIspiti_PopravniIspitId",
                table: "MaturskiIspitStavke",
                column: "PopravniIspitId",
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
    }
}
