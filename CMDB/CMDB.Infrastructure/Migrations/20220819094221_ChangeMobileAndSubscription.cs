using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMDB.Infrastructure.Migrations
{
    public partial class ChangeMobileAndSubscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Subscription",
                newName: "SubscriptionId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Mobile",
                newName: "MobileId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SubscriptionId",
                table: "Subscription",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "MobileId",
                table: "Mobile",
                newName: "Id");
        }
    }
}
