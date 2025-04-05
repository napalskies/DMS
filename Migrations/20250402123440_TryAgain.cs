using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDMS.Migrations
{
    /// <inheritdoc />
    public partial class TryAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Folder",
                table: "Documents",
                newName: "Folderinho");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Folderinho",
                table: "Documents",
                newName: "Folder");
        }
    }
}
