using Microsoft.EntityFrameworkCore;

namespace Pharmacie.Models
{


    public class Stock
    {
        public int Id { get; set; }
        public int MedicamentId { get; set; } // Foreign Key to Medicament
        public int Quantite { get; set; } // Quantity in stock
        public Medication Medicament { get; set; } = null!;

        // Navigation property to Medicament

    }
}


