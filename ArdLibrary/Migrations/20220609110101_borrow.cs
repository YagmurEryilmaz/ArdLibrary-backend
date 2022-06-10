using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdLibrary.Migrations
{
    public partial class borrow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthorName",
                table: "Borrows");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Borrows");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorName",
                table: "Borrows",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Title",
                table: "Borrows",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
