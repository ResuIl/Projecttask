using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecttask.Migrations
{
    /// <inheritdoc />
    public partial class updateOrders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_EmployerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_WorkerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_EmployerId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "EmployerId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Employer",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OfferPrice",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Worker",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Employer",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OfferPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Worker",
                table: "Orders");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Tag",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EmployerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkerId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EmployerId",
                table: "Orders",
                column: "EmployerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_WorkerId",
                table: "Orders",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_EmployerId",
                table: "Orders",
                column: "EmployerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_WorkerId",
                table: "Orders",
                column: "WorkerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
