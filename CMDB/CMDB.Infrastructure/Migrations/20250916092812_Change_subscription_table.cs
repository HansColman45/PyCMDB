using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Change_subscription_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Account",
                table: "Admin");

            migrationBuilder.RenameColumn(
                name: "SubsctiptionTypeId",
                table: "Subscription",
                newName: "SubscriptionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_SubsctiptionTypeId",
                table: "Subscription",
                newName: "IX_Subscription_SubscriptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Account",
                table: "Admin",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Account",
                table: "Admin");

            migrationBuilder.RenameColumn(
                name: "SubscriptionTypeId",
                table: "Subscription",
                newName: "SubsctiptionTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Subscription_SubscriptionTypeId",
                table: "Subscription",
                newName: "IX_Subscription_SubsctiptionTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Account",
                table: "Admin",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccID");
        }
    }
}
