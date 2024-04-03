using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library_management.Migrations
{
    /// <inheritdoc />
    public partial class addrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BooksISBN",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BooksISBN_BookId",
                table: "BooksISBN",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_BooksISBN_UserId",
                table: "BooksISBN",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksISBN_AspNetUsers_UserId",
                table: "BooksISBN",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BooksISBN_Books_BookId",
                table: "BooksISBN",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BooksISBN_AspNetUsers_UserId",
                table: "BooksISBN");

            migrationBuilder.DropForeignKey(
                name: "FK_BooksISBN_Books_BookId",
                table: "BooksISBN");

            migrationBuilder.DropIndex(
                name: "IX_BooksISBN_BookId",
                table: "BooksISBN");

            migrationBuilder.DropIndex(
                name: "IX_BooksISBN_UserId",
                table: "BooksISBN");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "BooksISBN",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }
    }
}
