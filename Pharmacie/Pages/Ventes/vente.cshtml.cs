using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Data;
using Pharmacie.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacie.Pages.Ventes
{
    public class  VenteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public VenteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Vente Vente { get; set; } = new();

        public List<Vente> Ventes { get; set; } = new List<Vente>();
        public List<Medication> Medications { get; set; } = new List<Medication>();

        public void OnGet(string medicamentName, string venteDate, int? medicamentId)
        {
            // Start with the base query for all ventes
            var query = _context.Ventes.AsQueryable();

            // If a medicament name is provided, filter by the medication name
            if (!string.IsNullOrEmpty(medicamentName))
            {
                query = query.Where(v => v.Medicament.Nom.Contains(medicamentName));
            }

            // If a medicamentId is selected, filter by the selected medicament
            if (medicamentId.HasValue)
            {
                query = query.Where(v => v.MedicamentId == medicamentId.Value);
            }

            // If a vente date is provided, filter by the vente date
            if (!string.IsNullOrEmpty(venteDate))
            {
                // Parse the vente date to DateTime
                if (DateTime.TryParse(venteDate, out var date))
                {
                    query = query.Where(v => v.Date.Date == date.Date);
                }
            }

            // Apply the query filter and load the filtered ventes
            Ventes = query.ToList();

            // Load medications list for the dropdown or any other purpose
            Medications = _context.Medicaments.ToList();
        }



        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                // Re-load data to show validation errors
                Ventes = _context.Ventes.Include(v => v.Medicament).ToList();
                Medications = _context.Medicaments.ToList();
                return Page();
            }

            // Ensure that the MedicamentId is valid
            var medicament = _context.Medicaments.Find(Vente.MedicamentId);
            if (medicament != null)
            {
                // Calculate Montant and Gain
                Vente.Montant = (decimal)((medicament.PrixBr ?? 0) * Vente.QuantiteVendu );
                Vente.Gain = (decimal)((medicament.PrixBr ?? 0) * Vente.QuantiteVendu );
                // You might want a different formula for Gain

                // Add new sale
                _context.Ventes.Add(Vente);
                _context.SaveChanges();
            }

            return RedirectToPage("/Ventes/vente");
        }
        public IActionResult OnPostSupprimer(int id)
        {
            var vente = _context.Ventes.Find(id);
            if (vente != null)
            {
                _context.Ventes.Remove(vente);
                _context.SaveChanges();
            }

            // Rediriger vers la même page pour actualiser la liste
            return RedirectToPage();
        }
        public IActionResult OnPostModifier(int id, DateTime date, int medicamentId, int quantiteVendu, decimal montant)
        {
            var vente = _context.Ventes.Find(id);
            if (vente != null)
            {
                vente.Date = date;
                vente.MedicamentId = medicamentId;
                vente.QuantiteVendu = quantiteVendu;
                var medicament = _context.Medicaments.Find(Vente.MedicamentId);
                vente.Montant = (decimal)((medicament.PrixBr ?? 0) * Vente.QuantiteVendu);
                vente.Gain = (decimal)((medicament.PrixBr ?? 0) * Vente.QuantiteVendu);

                _context.SaveChanges();
            }

            return RedirectToPage(); // Recharge la page pour actualiser les données
        }

    }
}
