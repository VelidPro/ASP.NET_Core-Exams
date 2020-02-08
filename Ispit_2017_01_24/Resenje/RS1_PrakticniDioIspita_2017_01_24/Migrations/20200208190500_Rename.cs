using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace RS1_PrakticniDioIspita_2017_01_24.Migrations
{
    public partial class Rename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationToken_User_UserId",
                table: "AuthorizationToken");

            migrationBuilder.DropForeignKey(
                name: "FK_Nastavnici_User_UserId",
                table: "Nastavnici");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorizationToken",
                table: "AuthorizationToken");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "AuthorizationToken",
                newName: "AuthorizationTokens");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorizationToken_UserId",
                table: "AuthorizationTokens",
                newName: "IX_AuthorizationTokens_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorizationTokens",
                table: "AuthorizationTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationTokens_Users_UserId",
                table: "AuthorizationTokens",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Nastavnici_Users_UserId",
                table: "Nastavnici",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AuthorizationTokens_Users_UserId",
                table: "AuthorizationTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_Nastavnici_Users_UserId",
                table: "Nastavnici");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuthorizationTokens",
                table: "AuthorizationTokens");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "AuthorizationTokens",
                newName: "AuthorizationToken");

            migrationBuilder.RenameIndex(
                name: "IX_AuthorizationTokens_UserId",
                table: "AuthorizationToken",
                newName: "IX_AuthorizationToken_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuthorizationToken",
                table: "AuthorizationToken",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AuthorizationToken_User_UserId",
                table: "AuthorizationToken",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Nastavnici_User_UserId",
                table: "Nastavnici",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
