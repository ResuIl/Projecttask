using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecttask.Migrations
{
    /// <inheritdoc />
    public partial class counter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "deletedOfferCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sentOfferCount",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deletedOfferCount",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "sentOfferCount",
                table: "AspNetUsers");
        }
    }
}
