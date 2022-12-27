using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WikipediaDAW.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "utilizatori",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rang = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_utilizatori", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "articole",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Domeniu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Titlu = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AutorcreareId = table.Column<int>(name: "Autor_creareId", type: "int", nullable: false),
                    Dataadaugarii = table.Column<DateTime>(name: "Data_adaugarii", type: "datetime2", nullable: false),
                    Continut = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Protejat = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_articole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_articole_utilizatori_Autor_creareId",
                        column: x => x.AutorcreareId,
                        principalTable: "utilizatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "istorici_admin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UtilizatorId = table.Column<int>(type: "int", nullable: false),
                    Actiune = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_istorici_admin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_istorici_admin_utilizatori_UtilizatorId",
                        column: x => x.UtilizatorId,
                        principalTable: "utilizatori",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "istorici_editare",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArticolId = table.Column<int>(type: "int", nullable: false),
                    Dataeditarii = table.Column<DateTime>(name: "Data_editarii", type: "datetime2", nullable: false),
                    Autoreditare = table.Column<string>(name: "Autor_editare", type: "nvarchar(max)", nullable: false),
                    Continut = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_istorici_editare", x => x.Id);
                    table.ForeignKey(
                        name: "FK_istorici_editare_articole_ArticolId",
                        column: x => x.ArticolId,
                        principalTable: "articole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_articole_Autor_creareId",
                table: "articole",
                column: "Autor_creareId");

            migrationBuilder.CreateIndex(
                name: "IX_istorici_admin_UtilizatorId",
                table: "istorici_admin",
                column: "UtilizatorId");

            migrationBuilder.CreateIndex(
                name: "IX_istorici_editare_ArticolId",
                table: "istorici_editare",
                column: "ArticolId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "istorici_admin");

            migrationBuilder.DropTable(
                name: "istorici_editare");

            migrationBuilder.DropTable(
                name: "articole");

            migrationBuilder.DropTable(
                name: "utilizatori");
        }
    }
}
