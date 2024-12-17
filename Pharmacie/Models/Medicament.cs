using Microsoft.EntityFrameworkCore;

namespace Pharmacie.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public string? Code { get; set; }       // Nullable
        public string? Nom { get; set; }        // Nullable
        public string? DCI1 { get; set; }       // Nullable
        public string? Dosage1 { get; set; }    // Nullable
        public string? UniteDosage1 { get; set; }
        public string? Forme { get; set; }
        public string? Presentation { get; set; }
        [Precision(18, 2)]
        public decimal? PPV { get; set; }       // Nullable

        [Precision(18, 2)]
        public decimal? PH { get; set; }        // Nullable

        [Precision(18, 2)]
        public decimal? PrixBr { get; set; }    // Nullable

        public string? PrincepsGenerique { get; set; }

        [Precision(18, 2)]
        public decimal? TauxRemboursement { get; set; }
    }

    public class Vente
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        [Precision(18, 2)]
        public decimal Montant { get; set; }

        [Precision(18, 2)]
        public decimal Gain { get; set; }

    
        public int QuantiteVendu { get; set; }

        // Nullable foreign key reference
        public int? MedicamentId { get; set; }
        public Medication? Medicament { get; set; }
       
    }

   





}
