using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDMS.Migrations
{
    /// <inheritdoc />
    public partial class TrialAndError : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Wtf",
                table: "Documents",
                type: "longblob",
                nullable: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Wtf",
                table: "Documents");
        }
    }
}
