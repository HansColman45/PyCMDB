using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class new_foreignkeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdenAccount_Account_AccountId",
                table: "IdenAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_IdenAccount_Identity_IdentityId",
                table: "IdenAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_IdenAccount_Account",
                table: "IdenAccount",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IdenAccount_Identity",
                table: "IdenAccount",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "IdenId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdenAccount_Account",
                table: "IdenAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_IdenAccount_Identity",
                table: "IdenAccount");

            migrationBuilder.AddForeignKey(
                name: "FK_IdenAccount_Account_AccountId",
                table: "IdenAccount",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_IdenAccount_Identity_IdentityId",
                table: "IdenAccount",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "IdenId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
