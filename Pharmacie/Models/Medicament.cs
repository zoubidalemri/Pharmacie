namespace Pharmacie.Models
{
    public class Medication
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Nom { get; set; }
        public string DCI1 { get; set; }
        public string Dosage1 { get; set; }
        public string UniteDosage1 { get; set; }
        public string Forme { get; set; }
        public string Presentation { get; set; }
        public decimal PPV { get; set; }
        public decimal PH { get; set; }
        public decimal PrixBr { get; set; }
        public string PrincepsGenerique { get; set; }
        public decimal TauxRemboursement { get; set; }
    }

    public class Vente
    {
        public int Id { get; set; }          // Identifiant de la vente
        public DateTime Date { get; set; }   // Date de la vente
        public decimal Montant { get; set; } // Montant total de la vente
        public decimal Gain { get; set; }    // Gain net de la vente (calculé en fonction de vos règles)

        // Référence au médicament
        public int MedicamentId { get; set; }   // Clé étrangère vers le médicament
        public Medication Medicament { get; set; }  // Navigation vers l'entité Medicament
    }



}
