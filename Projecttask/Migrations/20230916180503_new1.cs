using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecttask.Migrations
{
    /// <inheritdoc />
    public partial class new1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "UserCategories");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "UserCategories",
                newName: "CategoryIDId");

            migrationBuilder.AddColumn<string>(
                name: "UserIDId",
                table: "UserCategories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCategories_CategoryIDId",
                table: "UserCategories",
                column: "CategoryIDId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCategories_UserIDId",
                table: "UserCategories",
                column: "UserIDId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCategories_AspNetUsers_UserIDId",
                table: "UserCategories",
                column: "UserIDId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCategories_Categorie_CategoryIDId",
                table: "UserCategories",
                column: "CategoryIDId",
                principalTable: "Categorie",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCategories_AspNetUsers_UserIDId",
                table: "UserCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCategories_Categorie_CategoryIDId",
                table: "UserCategories");

            migrationBuilder.DropIndex(
                name: "IX_UserCategories_CategoryIDId",
                table: "UserCategories");

            migrationBuilder.DropIndex(
                name: "IX_UserCategories_UserIDId",
                table: "UserCategories");

            migrationBuilder.DropColumn(
                name: "UserIDId",
                table: "UserCategories");

            migrationBuilder.RenameColumn(
                name: "CategoryIDId",
                table: "UserCategories",
                newName: "UserID");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "UserCategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
