using Microsoft.EntityFrameworkCore.Migrations;

namespace GCD0901AppDev.Data.Migrations
{
    public partial class AddUserIdToTodoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Todoes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todoes_UserId",
                table: "Todoes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todoes_AspNetUsers_UserId",
                table: "Todoes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todoes_AspNetUsers_UserId",
                table: "Todoes");

            migrationBuilder.DropIndex(
                name: "IX_Todoes_UserId",
                table: "Todoes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Todoes");
        }
    }
}
