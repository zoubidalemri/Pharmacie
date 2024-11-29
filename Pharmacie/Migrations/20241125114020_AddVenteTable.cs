using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pharmacie.Migrations
{
    public partial class AddVenteTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ventes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(nullable: false),
                    Montant = table.Column<decimal>(nullable: false),
                    Gain = table.Column<decimal>(nullable: false),
                    QuantiteVendu = table.Column<int>(nullable: false),
                    MedicamentId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vente", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vente_Medicaments_MedicamentId",
                        column: x => x.MedicamentId,
                        principalTable: "Medicaments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ventes");
        }
    }


}
