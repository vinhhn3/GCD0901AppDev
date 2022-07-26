using Microsoft.EntityFrameworkCore.Migrations;

namespace GCD0901AppDev.Data.Migrations
{
    public partial class AddCategoriesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Todoes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Todoes_CategoryId",
                table: "Todoes",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todoes_Categories_CategoryId",
                table: "Todoes",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todoes_Categories_CategoryId",
                table: "Todoes");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Todoes_CategoryId",
                table: "Todoes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Todoes");
        }
    }
}
