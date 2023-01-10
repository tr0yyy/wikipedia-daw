using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WikipediaDAW.Migrations
{
    /// <inheritdoc />
    public partial class IstoricUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "istoric_articol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    articolId = table.Column<int>(type: "int", nullable: false),
                    continutvechi = table.Column<string>(name: "continut_vechi", type: "nvarchar(max)", nullable: false),
                    dataeditarii = table.Column<DateTime>(name: "data_editarii", type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_istoric_articol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_istoric_articol_articole_articolId",
                        column: x => x.articolId,
                        principalTable: "articole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_istoric_articol_articolId",
                table: "istoric_articol",
                column: "articolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "istoric_articol");
        }
    }
}
