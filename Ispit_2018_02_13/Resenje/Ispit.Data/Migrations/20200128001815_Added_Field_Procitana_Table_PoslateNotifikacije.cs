using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Ispit.Data.Migrations
{
    public partial class Added_Field_Procitana_Table_PoslateNotifikacije : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Procitana",
                table: "PoslataNotifikacija",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Procitana",
                table: "PoslataNotifikacija");
        }
    }
}
