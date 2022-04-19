using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArchivPoznamek.Migrations
{
    public partial class Poznamky : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Poznamky",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nadpis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Obsah = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DatumVlozeni = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Dulezitost = table.Column<bool>(type: "bit", nullable: false),
                    AutorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Poznamky", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Poznamky_Uzivatele_AutorId",
                        column: x => x.AutorId,
                        principalTable: "Uzivatele",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Poznamky_AutorId",
                table: "Poznamky",
                column: "AutorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Poznamky");
        }
    }
}
