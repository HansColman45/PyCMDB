using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedeletebehavior2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePerm_Menu",
                table: "RolePerm");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePerm_Permission",
                table: "RolePerm");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Category",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Type",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetType_Category",
                table: "SubscriptionType");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePerm_Menu",
                table: "RolePerm",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "MenuId");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePerm_Permission",
                table: "RolePerm",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Category",
                table: "Subscription",
                column: "AssetCategoryId",
                principalTable: "category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Type",
                table: "Subscription",
                column: "SubsctiptionTypeId",
                principalTable: "SubscriptionType",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AssetType_Category",
                table: "SubscriptionType",
                column: "AssetCategoryId",
                principalTable: "category",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RolePerm_Menu",
                table: "RolePerm");

            migrationBuilder.DropForeignKey(
                name: "FK_RolePerm_Permission",
                table: "RolePerm");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Category",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscription_Type",
                table: "Subscription");

            migrationBuilder.DropForeignKey(
                name: "FK_AssetType_Category",
                table: "SubscriptionType");

            migrationBuilder.AddForeignKey(
                name: "FK_RolePerm_Menu",
                table: "RolePerm",
                column: "MenuId",
                principalTable: "Menu",
                principalColumn: "MenuId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RolePerm_Permission",
                table: "RolePerm",
                column: "PermissionId",
                principalTable: "Permission",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Category",
                table: "Subscription",
                column: "AssetCategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscription_Type",
                table: "Subscription",
                column: "SubsctiptionTypeId",
                principalTable: "SubscriptionType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssetType_Category",
                table: "SubscriptionType",
                column: "AssetCategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
