using Microsoft.EntityFrameworkCore.Migrations;

namespace CMDB.Infrastructure.Migrations
{
    public partial class ChangeLanguageTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Language_Admin_LastModfiedAdminAdminId",
                table: "Language");

            migrationBuilder.DropForeignKey(
                name: "FK_Log_Language_LanguageCode",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Log_LanguageCode",
                table: "Log");

            migrationBuilder.DropIndex(
                name: "IX_Language_LastModfiedAdminAdminId",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "LanguageCode",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "Deactivate_reason",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "LastModfiedAdminAdminId",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "LastModifiedAdminId",
                table: "Language");

            migrationBuilder.DropColumn(
                name: "active",
                table: "Language");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanguageCode",
                table: "Log",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Deactivate_reason",
                table: "Language",
                type: "varchar(255)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModfiedAdminAdminId",
                table: "Language",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifiedAdminId",
                table: "Language",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "active",
                table: "Language",
                type: "int",
                maxLength: 1,
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Log_LanguageCode",
                table: "Log",
                column: "LanguageCode");

            migrationBuilder.CreateIndex(
                name: "IX_Language_LastModfiedAdminAdminId",
                table: "Language",
                column: "LastModfiedAdminAdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Language_Admin_LastModfiedAdminAdminId",
                table: "Language",
                column: "LastModfiedAdminAdminId",
                principalTable: "Admin",
                principalColumn: "Admin_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Log_Language_LanguageCode",
                table: "Log",
                column: "LanguageCode",
                principalTable: "Language",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
