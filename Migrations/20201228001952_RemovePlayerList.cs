using Microsoft.EntityFrameworkCore.Migrations;

namespace MindOfSpace_Api.Migrations
{
    public partial class RemovePlayerList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Player_Games_GameId",
                table: "Player");

            migrationBuilder.DropIndex(
                name: "IX_Player_GameId",
                table: "Player");

            migrationBuilder.DropColumn(
                name: "GameId",
                table: "Player");

            migrationBuilder.AlterColumn<string>(
                name: "GameId",
                table: "UserActivities",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GameKey",
                table: "Games",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameKey",
                table: "Games");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "UserActivities",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameId",
                table: "Player",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Player_GameId",
                table: "Player",
                column: "GameId");

            migrationBuilder.AddForeignKey(
                name: "FK_Player_Games_GameId",
                table: "Player",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
