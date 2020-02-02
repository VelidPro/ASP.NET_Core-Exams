using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class Added_Takmicenje_TakmicenjeUcesnik_Tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Takmicenja",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Razred = table.Column<int>(nullable: false),
                    DatumOdrzavanja = table.Column<DateTime>(nullable: false),
                    BrojPrijavljenih = table.Column<int>(nullable: false),
                    BrojKojiNisuPristupili = table.Column<int>(nullable: false),
                    SkolaDomacinId = table.Column<int>(nullable: false),
                    PredmetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Takmicenja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Takmicenja_Predmet_PredmetId",
                        column: x => x.PredmetId,
                        principalTable: "Predmet",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Takmicenja_Skola_SkolaDomacinId",
                        column: x => x.SkolaDomacinId,
                        principalTable: "Skola",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TakmicenjeUcesnici",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    OdjeljenjeStavkaId = table.Column<int>(nullable: false),
                    TakmicenjeId = table.Column<int>(nullable: false),
                    IsPristupio = table.Column<bool>(nullable: false),
                    OsvojeniBodovi = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TakmicenjeUcesnici", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TakmicenjeUcesnici_OdjeljenjeStavka_OdjeljenjeStavkaId",
                        column: x => x.OdjeljenjeStavkaId,
                        principalTable: "OdjeljenjeStavka",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TakmicenjeUcesnici_Takmicenja_TakmicenjeId",
                        column: x => x.TakmicenjeId,
                        principalTable: "Takmicenja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_PredmetId",
                table: "Takmicenja",
                column: "PredmetId");

            migrationBuilder.CreateIndex(
                name: "IX_Takmicenja_SkolaDomacinId",
                table: "Takmicenja",
                column: "SkolaDomacinId");

            migrationBuilder.CreateIndex(
                name: "IX_TakmicenjeUcesnici_OdjeljenjeStavkaId",
                table: "TakmicenjeUcesnici",
                column: "OdjeljenjeStavkaId");

            migrationBuilder.CreateIndex(
                name: "IX_TakmicenjeUcesnici_TakmicenjeId",
                table: "TakmicenjeUcesnici",
                column: "TakmicenjeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TakmicenjeUcesnici");

            migrationBuilder.DropTable(
                name: "Takmicenja");
        }
    }
}
