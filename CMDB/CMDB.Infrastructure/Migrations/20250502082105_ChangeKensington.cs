using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKensington : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Key_Device",
                table: "Kensington");

            migrationBuilder.DropIndex(
                name: "IX_Kensington_DeviceAssetTag",
                table: "Kensington");

            migrationBuilder.DropColumn(
                name: "DeviceAssetTag",
                table: "Kensington");

            migrationBuilder.AlterColumn<string>(
                name: "AssetTag",
                table: "Kensington",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kensington_AssetTag",
                table: "Kensington",
                column: "AssetTag",
                unique: true,
                filter: "[AssetTag] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Key_Device",
                table: "Kensington",
                column: "AssetTag",
                principalTable: "asset",
                principalColumn: "AssetTag",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Key_Device",
                table: "Kensington");

            migrationBuilder.DropIndex(
                name: "IX_Kensington_AssetTag",
                table: "Kensington");

            migrationBuilder.AlterColumn<string>(
                name: "AssetTag",
                table: "Kensington",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeviceAssetTag",
                table: "Kensington",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Kensington_DeviceAssetTag",
                table: "Kensington",
                column: "DeviceAssetTag");

            migrationBuilder.AddForeignKey(
                name: "FK_Key_Device",
                table: "Kensington",
                column: "DeviceAssetTag",
                principalTable: "asset",
                principalColumn: "AssetTag",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
