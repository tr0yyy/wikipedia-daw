using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WikipediaDAW.Migrations
{
    /// <inheritdoc />
    public partial class DomeniiSeparate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Domeniu",
                table: "articole");

            migrationBuilder.AddColumn<int>(
                name: "DomeniuId",
                table: "articole",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "domeniu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_domeniu", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_articole_DomeniuId",
                table: "articole",
                column: "DomeniuId");

            migrationBuilder.AddForeignKey(
                name: "FK_articole_domeniu_DomeniuId",
                table: "articole",
                column: "DomeniuId",
                principalTable: "domeniu",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_articole_domeniu_DomeniuId",
                table: "articole");

            migrationBuilder.DropTable(
                name: "domeniu");

            migrationBuilder.DropIndex(
                name: "IX_articole_DomeniuId",
                table: "articole");

            migrationBuilder.DropColumn(
                name: "DomeniuId",
                table: "articole");

            migrationBuilder.AddColumn<string>(
                name: "Domeniu",
                table: "articole",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
