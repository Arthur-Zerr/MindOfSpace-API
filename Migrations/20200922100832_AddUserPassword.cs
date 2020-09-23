using Microsoft.EntityFrameworkCore.Migrations;

namespace MindOfSpace_Api.Migrations
{
    public partial class AddUserPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DateSalt",
                table: "Players",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Players",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateSalt",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Players");
        }
    }
}
