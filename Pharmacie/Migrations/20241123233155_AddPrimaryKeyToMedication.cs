using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie.Migrations
{
    public partial class AddPrimaryKeyToMedication : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Medicaments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DCI1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dosage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UniteDosage1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Forme = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Presentation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PPV = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PH = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrixBr = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrincepsGenerique = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TauxRemboursement = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medicaments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Ventes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Gain = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MedicamentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ventes_Medicaments_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Ventes_MedicamentId",
                table: "Ventes",
                column: "MedicamentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ventes");

            migrationBuilder.DropTable(
                name: "Medicaments");
        }
    }
}
