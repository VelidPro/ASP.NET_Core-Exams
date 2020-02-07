using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_Ispit_asp.net_core.Migrations
{
    public partial class Edit_MaturskiIspit_OsvojeniBodovi_Nullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OsvojeniBodovi",
                table: "MaturskiIspitStavke",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OsvojeniBodovi",
                table: "MaturskiIspitStavke",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
