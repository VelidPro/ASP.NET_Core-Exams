using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nastavnik",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(nullable: true),
                    Prezime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nastavnik", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predmet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true),
                    Razred = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skola",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skola", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkolskaGodina",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Aktuelna = table.Column<bool>(nullable: false),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkolskaGodina", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ucenik",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImePrezime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ucenik", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Odjeljenje",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    IsPrebacenuViseOdjeljenje = table.Column<bool>(nullable: false),
                    Oznaka = table.Column<string>(nullable: true),
                    Razred = table.Column<int>(nullable: false),
                    RazrednikID = table.Column<int>(nullable: false),
                    SkolaID = table.Column<int>(nullable: false),
                    SkolskaGodinaID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odjeljenje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odjeljenje_Nastavnik_RazrednikID",
                        column: x => x.RazrednikID,
                        principalTable: "Nastavnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Odjeljenje_Skola_SkolaID",
                        column: x => x.SkolaID,
                        principalTable: "Skola",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Odjeljenje_SkolskaGodina_SkolskaGodinaID",
                        column: x => x.SkolskaGodinaID,
                        principalTable: "SkolskaGodina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PopravniIspiti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DatumOdrzavanja = table.Column<DateTime>(nullable: false),
                    Napomena = table.Column<string>(nullable: true),
                    PredmetId = table.Column<int>(nullable: false),
                    SkolaId = table.Column<int>(nullable: false),
                    SkolskaGodinaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopravniIspiti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PopravniIspiti_Predmet_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PopravniIspiti_Skola_SkolaId",
                        column: x => x.SkolaId,
                        principalTable: "Skola",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PopravniIspiti_SkolskaGodina_SkolskaGodinaId",
                        column: x => x.SkolskaGodinaId,
                        principalTable: "SkolskaGodina",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OdjeljenjeStavka",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrojUDnevniku = table.Column<int>(nullable: false),
                    OdjeljenjeId = table.Column<int>(nullable: false),
                    UcenikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdjeljenjeStavka", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdjeljenjeStavka_Odjeljenje_OdjeljenjeId",
                        column: x => x.OdjeljenjeId,
                        principalTable: "Odjeljenje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OdjeljenjeStavka_Ucenik_UcenikId",
                        column: x => x.UcenikId,
                        principalTable: "Ucenik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PredajePredmet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NastavnikID = table.Column<int>(nullable: false),
                    OdjeljenjeID = table.Column<int>(nullable: false),
                    PredmetID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PredajePredmet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PredajePredmet_Nastavnik_NastavnikID",
                        column: x => x.NastavnikID,
                        principalTable: "Nastavnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PredajePredmet_Odjeljenje_OdjeljenjeID",
                        column: x => x.OdjeljenjeID,
                        principalTable: "Odjeljenje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PredajePredmet_Predmet_PredmetID",
                        column: x => x.PredmetID,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PopravniIspitKomisija_PopravniIspiti_PopravniIspitId",
                        column: x => x.PopravniIspitId,
                        principalTable: "PopravniIspiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PopravniIspitStavke",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImaPravoNaIzlazask = table.Column<bool>(nullable: false),
                    IsPrisutupio = table.Column<bool>(nullable: false),
                    OsvojeniBodovi = table.Column<int>(nullable: true),
                    PopravniIspitId = table.Column<int>(nullable: false),
                    UcenikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PopravniIspitStavke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PopravniIspitStavke_PopravniIspiti_PopravniIspitId",
                        column: x => x.PopravniIspitId,
                        principalTable: "PopravniIspiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PopravniIspitStavke_Ucenik_UcenikId",
                        column: x => x.UcenikId,
                        principalTable: "Ucenik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DodjeljenPredmet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OdjeljenjeStavkaId = table.Column<int>(nullable: false),
                    PredmetId = table.Column<int>(nullable: false),
                    ZakljucnoKrajGodine = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DodjeljenPredmet", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DodjeljenPredmet_OdjeljenjeStavka_OdjeljenjeStavkaId",
                        column: x => x.OdjeljenjeStavkaId,
                        principalTable: "OdjeljenjeStavka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DodjeljenPredmet_Predmet_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DodjeljenPredmet_OdjeljenjeStavkaId",
                table: "DodjeljenPredmet",
                column: "OdjeljenjeStavkaId");

            migrationBuilder.CreateIndex(
                name: "IX_DodjeljenPredmet_PredmetId",
                table: "DodjeljenPredmet",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Odjeljenje_RazrednikID",
                table: "Odjeljenje",
                column: "RazrednikID");

            migrationBuilder.CreateIndex(
                name: "IX_Odjeljenje_SkolaID",
                table: "Odjeljenje",
                column: "SkolaID");

            migrationBuilder.CreateIndex(
                name: "IX_Odjeljenje_SkolskaGodinaID",
                table: "Odjeljenje",
                column: "SkolskaGodinaID");

            migrationBuilder.CreateIndex(
                name: "IX_OdjeljenjeStavka_OdjeljenjeId",
                table: "OdjeljenjeStavka",
                column: "OdjeljenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_OdjeljenjeStavka_UcenikId",
                table: "OdjeljenjeStavka",
                column: "UcenikId");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspiti_PredmetId",
                table: "PopravniIspiti",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspiti_SkolaId",
                table: "PopravniIspiti",
                column: "SkolaId");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspiti_SkolskaGodinaId",
                table: "PopravniIspiti",
                column: "SkolskaGodinaId");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspitKomisija_NastavnikId",
                table: "PopravniIspitKomisija",
                column: "NastavnikId");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspitKomisija_PopravniIspitId",
                table: "PopravniIspitKomisija",
                column: "PopravniIspitId");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspitStavke_PopravniIspitId",
                table: "PopravniIspitStavke",
                column: "PopravniIspitId");

            migrationBuilder.CreateIndex(
                name: "IX_PopravniIspitStavke_UcenikId",
                table: "PopravniIspitStavke",
                column: "UcenikId");

            migrationBuilder.CreateIndex(
                name: "IX_PredajePredmet_NastavnikID",
                table: "PredajePredmet",
                column: "NastavnikID");

            migrationBuilder.CreateIndex(
                name: "IX_PredajePredmet_OdjeljenjeID",
                table: "PredajePredmet",
                column: "OdjeljenjeID");

            migrationBuilder.CreateIndex(
                name: "IX_PredajePredmet_PredmetID",
                table: "PredajePredmet",
                column: "PredmetID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DodjeljenPredmet");

            migrationBuilder.DropTable(
                name: "PopravniIspitKomisija");

            migrationBuilder.DropTable(
                name: "PopravniIspitStavke");

            migrationBuilder.DropTable(
                name: "PredajePredmet");

            migrationBuilder.DropTable(
                name: "OdjeljenjeStavka");

            migrationBuilder.DropTable(
                name: "PopravniIspiti");

            migrationBuilder.DropTable(
                name: "Odjeljenje");

            migrationBuilder.DropTable(
                name: "Ucenik");

            migrationBuilder.DropTable(
                name: "Predmet");

            migrationBuilder.DropTable(
                name: "Nastavnik");

            migrationBuilder.DropTable(
                name: "Skola");

            migrationBuilder.DropTable(
                name: "SkolskaGodina");
        }
    }
}
