using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class change_log_column_name : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Log_RolePerm",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "SubsriptionId",
                table: "Log",
                newName: "SubscriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Log_SubsriptionId",
                table: "Log",
                newName: "IX_Log_SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Log_RolePermId",
                table: "Log",
                column: "RolePermId");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_RolePerm",
                table: "Log",
                column: "RolePermId",
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

            migrationBuilder.DropIndex(
                name: "IX_Log_RolePermId",
                table: "Log");

            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "Log",
                newName: "SubsriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Log_SubscriptionId",
                table: "Log",
                newName: "IX_Log_SubsriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Log_RolePerm",
                table: "Log",
                column: "RoleId",
                principalTable: "RolePerm",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
