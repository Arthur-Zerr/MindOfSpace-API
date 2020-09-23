using Microsoft.EntityFrameworkCore.Migrations;

namespace MindOfSpace_Api.Migrations
{
    public partial class AddGameDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "UserActivities",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameId",
                table: "UserActivities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Games");
        }
    }
}
