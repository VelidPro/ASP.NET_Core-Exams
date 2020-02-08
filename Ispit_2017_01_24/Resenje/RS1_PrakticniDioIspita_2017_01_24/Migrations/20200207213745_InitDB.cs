using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_PrakticniDioIspita_2017_01_24.Migrations
{
    public partial class InitDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Nastavnici",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(nullable: true),
                    Password = table.Column<string>(nullable: true),
                    Username = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nastavnici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Predmeti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Predmeti", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ucenici",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ucenici", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Odjeljenja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NastavnikId = table.Column<int>(nullable: false),
                    Oznaka = table.Column<string>(nullable: true),
                    Razred = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odjeljenja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odjeljenja_Nastavnici_NastavnikId",
                        column: x => x.NastavnikId,
                        principalTable: "Nastavnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Angazovani",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NastavnikId = table.Column<int>(nullable: false),
                    OdjeljenjeId = table.Column<int>(nullable: false),
                    PredmetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Angazovani", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Angazovani_Nastavnici_NastavnikId",
                        column: x => x.NastavnikId,
                        principalTable: "Nastavnici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Angazovani_Odjeljenja_OdjeljenjeId",
                        column: x => x.OdjeljenjeId,
                        principalTable: "Odjeljenja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Angazovani_Predmeti_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmeti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UpisiUOdjeljenja",
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
                    table.PrimaryKey("PK_UpisiUOdjeljenja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpisiUOdjeljenja_Odjeljenja_OdjeljenjeId",
                        column: x => x.OdjeljenjeId,
                        principalTable: "Odjeljenja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UpisiUOdjeljenja_Ucenici_UcenikId",
                        column: x => x.UcenikId,
                        principalTable: "Ucenici",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OdrzaniCasovi",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AngazovanId = table.Column<int>(nullable: false),
                    datum = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdrzaniCasovi", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdrzaniCasovi_Angazovani_AngazovanId",
                        column: x => x.AngazovanId,
                        principalTable: "Angazovani",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OdrzaniCasDetalji",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Ocjena = table.Column<int>(nullable: true),
                    OdrzaniCasId = table.Column<int>(nullable: false),
                    Odsutan = table.Column<bool>(nullable: false),
                    OpravdanoOdsutan = table.Column<bool>(nullable: true),
                    UpisUOdjeljenjeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OdrzaniCasDetalji", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OdrzaniCasDetalji_OdrzaniCasovi_OdrzaniCasId",
                        column: x => x.OdrzaniCasId,
                        principalTable: "OdrzaniCasovi",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OdrzaniCasDetalji_UpisiUOdjeljenja_UpisUOdjeljenjeId",
                        column: x => x.UpisUOdjeljenjeId,
                        principalTable: "UpisiUOdjeljenja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Angazovani_NastavnikId",
                table: "Angazovani",
                column: "NastavnikId");

            migrationBuilder.CreateIndex(
                name: "IX_Angazovani_OdjeljenjeId",
                table: "Angazovani",
                column: "OdjeljenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_Angazovani_PredmetId",
                table: "Angazovani",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Odjeljenja_NastavnikId",
                table: "Odjeljenja",
                column: "NastavnikId");

            migrationBuilder.CreateIndex(
                name: "IX_OdrzaniCasDetalji_OdrzaniCasId",
                table: "OdrzaniCasDetalji",
                column: "OdrzaniCasId");

            migrationBuilder.CreateIndex(
                name: "IX_OdrzaniCasDetalji_UpisUOdjeljenjeId",
                table: "OdrzaniCasDetalji",
                column: "UpisUOdjeljenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_OdrzaniCasovi_AngazovanId",
                table: "OdrzaniCasovi",
                column: "AngazovanId");

            migrationBuilder.CreateIndex(
                name: "IX_UpisiUOdjeljenja_OdjeljenjeId",
                table: "UpisiUOdjeljenja",
                column: "OdjeljenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_UpisiUOdjeljenja_UcenikId",
                table: "UpisiUOdjeljenja",
                column: "UcenikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OdrzaniCasDetalji");

            migrationBuilder.DropTable(
                name: "OdrzaniCasovi");

            migrationBuilder.DropTable(
                name: "UpisiUOdjeljenja");

            migrationBuilder.DropTable(
                name: "Angazovani");

            migrationBuilder.DropTable(
                name: "Ucenici");

            migrationBuilder.DropTable(
                name: "Odjeljenja");

            migrationBuilder.DropTable(
                name: "Predmeti");

            migrationBuilder.DropTable(
                name: "Nastavnici");
        }
    }
}
