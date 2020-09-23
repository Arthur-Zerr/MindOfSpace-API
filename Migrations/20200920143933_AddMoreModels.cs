using Microsoft.EntityFrameworkCore.Migrations;

namespace MindOfSpace_Api.Migrations
{
    public partial class AddMoreModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Players",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Highscores",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsDeleted = table.Column<bool>(nullable: true),
                    PlayerId = table.Column<int>(nullable: false),
                    HighScore = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Highscores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Highscores_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Highscores_PlayerId",
                table: "Highscores",
                column: "PlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Highscores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Players");
        }
    }
}
