namespace Pharmacie.Models
{
    public class Medication
    {
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

}
