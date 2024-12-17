using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacie.Data;
using Pharmacie.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacie.Pages.Medications
{
    public class IndexxModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexxModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Medication Medication { get; set; } = new();

        public List<Medication> Medications { get; set; } = new();

        public void OnGet(int? editId, int? deleteId)
        {
            // Charger la liste des médicaments
            Medications = _context.Medicaments.ToList();

            // Si un ID de modification est fourni, charger l'objet correspondant
            if (editId.HasValue)
            {
                Medication = _context.Medicaments.Find(editId.Value);
            }
            else
            {
                Medication = new Medication(); // Objet vide pour ajout
            }

            // Si un ID de suppression est fourni, supprimer l'objet correspondant
            if (deleteId.HasValue)
            {
                var medicamentToDelete = _context.Medicaments.Find(deleteId.Value);
                if (medicamentToDelete != null)
                {
                    _context.Medicaments.Remove(medicamentToDelete);
                    _context.SaveChanges();
                }

                // Recharger la liste après suppression
                Medications = _context.Medicaments.ToList();
            }
        }

       

        public IActionResult OnPost()
        {
            if (Medication.Id > 0) // Modifier un médicament existant
            {
                var medicamentToUpdate = _context.Medicaments.Find(Medication.Id);
                if (medicamentToUpdate != null)
                {
                    // Mettre à jour les champs avec les nouvelles valeurs du formulaire
                    medicamentToUpdate.Code = Medication.Code;
                    medicamentToUpdate.Nom = Medication.Nom;
                    medicamentToUpdate.DCI1 = Medication.DCI1;
                    medicamentToUpdate.Dosage1 = Medication.Dosage1;
                    medicamentToUpdate.UniteDosage1 = Medication.UniteDosage1;
                    medicamentToUpdate.Forme = Medication.Forme;
                    medicamentToUpdate.Presentation = Medication.Presentation;
                    medicamentToUpdate.PPV = Medication.PPV;
                    medicamentToUpdate.PH = Medication.PH;
                    medicamentToUpdate.PrixBr = Medication.PrixBr;
                    medicamentToUpdate.PrincepsGenerique = Medication.PrincepsGenerique;
                    medicamentToUpdate.TauxRemboursement = Medication.TauxRemboursement;

                    _context.SaveChanges();
                }
            }
            else // Ajouter un nouveau médicament
            {
                _context.Medicaments.Add(Medication);
                _context.SaveChanges();
            }

        

            // Rediriger pour éviter le double POST
            return RedirectToPage("/Medications/index");
        }
    }
}
