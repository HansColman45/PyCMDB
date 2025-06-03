using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class NewLogColums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RolePermId",
                table: "Log",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Log_RolePerm",
                table: "Log",
                column: "RoleId",
                principalTable: "RolePerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_RolePerm",
                table: "Log");

            migrationBuilder.DropColumn(
                name: "RolePermId",
                table: "Log");
        }
    }
}
