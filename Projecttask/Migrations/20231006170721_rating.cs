using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Projecttask.Migrations
{
    /// <inheritdoc />
    public partial class rating : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isJobAccepted",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isJobFinished",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isJobRated",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "rating",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isJobAccepted",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "isJobFinished",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "isJobRated",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "rating",
                table: "Orders");
        }
    }
}
