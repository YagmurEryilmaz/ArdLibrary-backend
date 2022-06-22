using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArdLibrary.Migrations
{
    public partial class prewBorrow : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrevBorrows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    BookId = table.Column<int>(type: "int", nullable: false),
                    ExpDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrevBorrows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PrevBorrows_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PrevBorrows_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PrevBorrows_BookId",
                table: "PrevBorrows",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_PrevBorrows_UserId",
                table: "PrevBorrows",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrevBorrows");
        }
    }
}
