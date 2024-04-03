using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace library_management.Migrations
{
    /// <inheritdoc />
    public partial class addnewfieldisbn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "Borrowings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "Borrowings");
        }
    }
}
