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
                Medication = _context.Medicaments.Find(editId.Value) ?? new Medication();
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
            if (!ModelState.IsValid)
            {
                Medications = _context.Medicaments.ToList();
                return Page();
            }

            if (Medication.Id == 0)
            {
                // Ajouter un nouveau médicament
                _context.Medicaments.Add(Medication);
            }
            else
            {
                // Modifier un médicament existant
                _context.Medicaments.Update(Medication);
            }

            _context.SaveChanges();

            // Rediriger pour éviter le double POST
            return RedirectToPage("/Medications/Indexx");
        }
    }
}
