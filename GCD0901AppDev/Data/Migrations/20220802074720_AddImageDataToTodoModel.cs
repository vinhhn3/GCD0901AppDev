using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GCD0901AppDev.Data.Migrations
{
    public partial class AddImageDataToTodoModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageData",
                table: "Todoes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageData",
                table: "Todoes");
        }
    }
}
