using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WikipediaDAW.Migrations
{
    /// <inheritdoc />
    public partial class Useroptionalonarticol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_articole_AspNetUsers_AutorId",
                table: "articole");

            migrationBuilder.AlterColumn<string>(
                name: "AutorId",
                table: "articole",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_articole_AspNetUsers_AutorId",
                table: "articole",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_articole_AspNetUsers_AutorId",
                table: "articole");

            migrationBuilder.AlterColumn<string>(
                name: "AutorId",
                table: "articole",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_articole_AspNetUsers_AutorId",
                table: "articole",
                column: "AutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
