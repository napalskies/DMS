using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyDMS.Migrations
{
    /// <inheritdoc />
    public partial class MakeFolderOwnerNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_FolderOwnerId",
                table: "Folders");

            migrationBuilder.AlterColumn<string>(
                name: "FolderOwnerId",
                table: "Folders",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_FolderOwnerId",
                table: "Folders",
                column: "FolderOwnerId",
                principalTable: "Folders",
                principalColumn: "FolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Folders_Folders_FolderOwnerId",
                table: "Folders");

            migrationBuilder.UpdateData(
                table: "Folders",
                keyColumn: "FolderOwnerId",
                keyValue: null,
                column: "FolderOwnerId",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "FolderOwnerId",
                table: "Folders",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_Folders_Folders_FolderOwnerId",
                table: "Folders",
                column: "FolderOwnerId",
                principalTable: "Folders",
                principalColumn: "FolderId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
