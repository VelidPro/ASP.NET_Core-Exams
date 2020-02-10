using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_2017_06_21_v1.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Password = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuthorizationTokens",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IpAddress = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthorizationTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthorizationTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Nastavnik",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ImePrezime = table.Column<string>(nullable: true),
                    UserId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Nastavnik", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Nastavnik_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Odjeljenje",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NastavnikId = table.Column<int>(nullable: false),
                    Naziv = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Odjeljenje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Odjeljenje_Nastavnik_NastavnikId",
                        column: x => x.NastavnikId,
                        principalTable: "Nastavnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaturskiIspiti",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Datum = table.Column<DateTime>(nullable: false),
                    NastavnikId = table.Column<int>(nullable: false),
                    OdjeljenjeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaturskiIspiti", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaturskiIspiti_Nastavnik_NastavnikId",
                        column: x => x.NastavnikId,
                        principalTable: "Nastavnik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaturskiIspiti_Odjeljenje_OdjeljenjeId",
                        column: x => x.OdjeljenjeId,
                        principalTable: "Odjeljenje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UpisUOdjeljenje",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    BrojUDnevniku = table.Column<int>(nullable: false),
                    OdjeljenjeId = table.Column<int>(nullable: false),
                    OpciUspjeh = table.Column<int>(nullable: false),
                    UcenikId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UpisUOdjeljenje", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UpisUOdjeljenje_Odjeljenje_OdjeljenjeId",
                        column: x => x.OdjeljenjeId,
                        principalTable: "Odjeljenje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UpisUOdjeljenje_Ucenik_UcenikId",
                        column: x => x.UcenikId,
                        principalTable: "Ucenik",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MaturskiIspitStavke",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Bodovi = table.Column<float>(nullable: true),
                    MaturskiIspitId = table.Column<int>(nullable: false),
                    Oslobodjen = table.Column<bool>(nullable: false),
                    UpisUOdjeljenjeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MaturskiIspitStavke", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MaturskiIspitStavke_MaturskiIspiti_MaturskiIspitId",
                        column: x => x.MaturskiIspitId,
                        principalTable: "MaturskiIspiti",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MaturskiIspitStavke_UpisUOdjeljenje_UpisUOdjeljenjeId",
                        column: x => x.UpisUOdjeljenjeId,
                        principalTable: "UpisUOdjeljenje",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthorizationTokens_UserId",
                table: "AuthorizationTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MaturskiIspiti_NastavnikId",
                table: "MaturskiIspiti",
                column: "NastavnikId");

            migrationBuilder.CreateIndex(
                name: "IX_MaturskiIspiti_OdjeljenjeId",
                table: "MaturskiIspiti",
                column: "OdjeljenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_MaturskiIspitStavke_MaturskiIspitId",
                table: "MaturskiIspitStavke",
                column: "MaturskiIspitId");

            migrationBuilder.CreateIndex(
                name: "IX_MaturskiIspitStavke_UpisUOdjeljenjeId",
                table: "MaturskiIspitStavke",
                column: "UpisUOdjeljenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_Nastavnik_UserId",
                table: "Nastavnik",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Odjeljenje_NastavnikId",
                table: "Odjeljenje",
                column: "NastavnikId");

            migrationBuilder.CreateIndex(
                name: "IX_UpisUOdjeljenje_OdjeljenjeId",
                table: "UpisUOdjeljenje",
                column: "OdjeljenjeId");

            migrationBuilder.CreateIndex(
                name: "IX_UpisUOdjeljenje_UcenikId",
                table: "UpisUOdjeljenje",
                column: "UcenikId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthorizationTokens");

            migrationBuilder.DropTable(
                name: "MaturskiIspitStavke");

            migrationBuilder.DropTable(
                name: "MaturskiIspiti");

            migrationBuilder.DropTable(
                name: "UpisUOdjeljenje");

            migrationBuilder.DropTable(
                name: "Odjeljenje");

            migrationBuilder.DropTable(
                name: "Ucenik");

            migrationBuilder.DropTable(
                name: "Nastavnik");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
