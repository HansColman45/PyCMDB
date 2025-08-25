using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CMDB.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDeletebehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Application",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Type",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Account",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Admin_LastModiefiedAdmin",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_IdenAccount_Account",
                table: "IdenAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_IdenAccount_Identity",
                table: "IdenAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Identity_Language",
                table: "Identity");

            migrationBuilder.DropForeignKey(
                name: "FK_Identity_Type",
                table: "Identity");

            migrationBuilder.DropForeignKey(
                name: "FK_Kensington_Category",
                table: "Kensington");

            migrationBuilder.DropForeignKey(
                name: "FK_Kensington_Type",
                table: "Kensington");

            migrationBuilder.DropForeignKey(
                name: "FK_Mobile_Category",
                table: "Mobile");

            migrationBuilder.DropForeignKey(
                name: "FK_Mobile_Type",
                table: "Mobile");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Type",
                table: "Role");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Application",
                table: "Account",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "AppID");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Type",
                table: "Account",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Account",
                table: "Admin",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccID");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_LastModiefiedAdmin",
                table: "Admin",
                column: "LastModifiedAdminId",
                principalTable: "Admin",
                principalColumn: "Admin_id");

            migrationBuilder.AddForeignKey(
                name: "FK_IdenAccount_Account",
                table: "IdenAccount",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccID");

            migrationBuilder.AddForeignKey(
                name: "FK_IdenAccount_Identity",
                table: "IdenAccount",
                column: "IdentityId",
                principalTable: "Identity",
                principalColumn: "IdenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_Language",
                table: "Identity",
                column: "LanguageCode",
                principalTable: "Language",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_Type",
                table: "Identity",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Kensington_Category",
                table: "Kensington",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Kensington_Type",
                table: "Kensington",
                column: "TypeId",
                principalTable: "AssetType",
                principalColumn: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Mobile_Category",
                table: "Mobile",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Mobile_Type",
                table: "Mobile",
                column: "TypeId",
                principalTable: "AssetType",
                principalColumn: "TypeID");

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Type",
                table: "Role",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Account_Application",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Account_Type",
                table: "Account");

            migrationBuilder.DropForeignKey(
                name: "FK_Admin_Account",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_Admin_LastModiefiedAdmin",
                table: "Admin");

            migrationBuilder.DropForeignKey(
                name: "FK_IdenAccount_Account",
                table: "IdenAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_IdenAccount_Identity",
                table: "IdenAccount");

            migrationBuilder.DropForeignKey(
                name: "FK_Identity_Language",
                table: "Identity");

            migrationBuilder.DropForeignKey(
                name: "FK_Identity_Type",
                table: "Identity");

            migrationBuilder.DropForeignKey(
                name: "FK_Kensington_Category",
                table: "Kensington");

            migrationBuilder.DropForeignKey(
                name: "FK_Kensington_Type",
                table: "Kensington");

            migrationBuilder.DropForeignKey(
                name: "FK_Mobile_Category",
                table: "Mobile");

            migrationBuilder.DropForeignKey(
                name: "FK_Mobile_Type",
                table: "Mobile");

            migrationBuilder.DropForeignKey(
                name: "FK_Role_Type",
                table: "Role");

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Application",
                table: "Account",
                column: "ApplicationId",
                principalTable: "Application",
                principalColumn: "AppID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Account_Type",
                table: "Account",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_Account",
                table: "Admin",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "AccID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_LastModiefiedAdmin",
                table: "Admin",
                column: "LastModifiedAdminId",
                principalTable: "Admin",
                principalColumn: "Admin_id",
                onDelete: ReferentialAction.Restrict);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_Language",
                table: "Identity",
                column: "LanguageCode",
                principalTable: "Language",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Identity_Type",
                table: "Identity",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kensington_Category",
                table: "Kensington",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Kensington_Type",
                table: "Kensington",
                column: "TypeId",
                principalTable: "AssetType",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mobile_Category",
                table: "Mobile",
                column: "CategoryId",
                principalTable: "category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Mobile_Type",
                table: "Mobile",
                column: "TypeId",
                principalTable: "AssetType",
                principalColumn: "TypeID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Role_Type",
                table: "Role",
                column: "TypeId",
                principalTable: "Type",
                principalColumn: "TypeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
