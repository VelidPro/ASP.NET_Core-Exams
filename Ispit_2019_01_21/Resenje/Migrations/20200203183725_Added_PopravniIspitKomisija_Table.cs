using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class Added_PopravniIspitKomisija_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PopravniIspiti_Nastavnik_NastavnikId",
                table: "PopravniIspiti");

            migrationBuilder.DropIndex(
                name: "IX_PopravniIspiti_NastavnikId",
                table: "PopravniIspiti");

            migrationBuilder.DropColumn(
                name: "NastavnikId",
                table: "PopravniIspiti");

            migrationBuilder.AlterColumn<int>(
                name: "OsvojeniBodovi",
                table: "PopravniIspitStavke",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "PopravniIspitKomisija",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NastavnikId = table.Column<int>(nullable: false),
                    PopravniIspitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopravniIspitKomisija", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PopravniIspitKomisija_Nastavnik_NastavnikId",
                        column: x => x.NastavnikId,
                        principalTable: "Nastavnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PopravniIspitKomisija_PopravniIspiti_PopravniIspitId",
                        column: x => x.PopravniIspitId,
                        principalTable: "PopravniIspiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspitKomisija_NastavnikId",
                table: "PopravniIspitKomisija",
                column: "NastavnikId");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspitKomisija_PopravniIspitId",
                table: "PopravniIspitKomisija",
                column: "PopravniIspitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PopravniIspitKomisija");

            migrationBuilder.AlterColumn<int>(
                name: "OsvojeniBodovi",
                table: "PopravniIspitStavke",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NastavnikId",
                table: "PopravniIspiti",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspiti_NastavnikId",
                table: "PopravniIspiti",
                column: "NastavnikId");

            migrationBuilder.AddForeignKey(
                name: "FK_PopravniIspiti_Nastavnik_NastavnikId",
                table: "PopravniIspiti",
                column: "NastavnikId",
                principalTable: "Nastavnik",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
