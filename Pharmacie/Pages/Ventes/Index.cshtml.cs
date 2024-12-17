using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Models;
using Pharmacie.Data;
using static Microsoft.IO.RecyclableMemoryStreamManager;

namespace Pharmacie.Pages.Ventes
{
    public class venteModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public venteModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Vente Vente { get; set; } = new();

        public List<Vente> Ventes { get; set; } = new();
        public List<Medication> Medications { get; set; } = new();
        public List<Stock> Stocks { get; set; } = new();
        public string ErrorMessage { get; set; } = string.Empty;
        [BindProperty(SupportsGet = true)]
        public int? MedicamentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? VenteDate { get; set; }

        public async Task<IActionResult> OnGetAsync(string medicamentName, string venteDate, int? medicamentId)
        {
            // Start with the base query for all ventes
            var query = _context.Ventes.Include(v => v.Medicament).AsQueryable();

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
            Ventes = await query.ToListAsync();

            // Load medications list for the dropdown or any other purpose
            Medications = await _context.Medicaments.ToListAsync();

            return Page();
        }


        // Apply the query filter and load the filtered ventes


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Données invalides.";
                await OnGetAsync(null, null, null); // Call OnGetAsync with default parameters
                return Page();
            }

            var stock = await _context.Set<Stock>().FirstOrDefaultAsync(s => s.MedicamentId == Vente.MedicamentId);
            if (stock == null)
            {
                ErrorMessage = "Médicament non trouvé en stock.";
                await OnGetAsync(null, null, null);
                return Page();
            }

            if (stock.Quantite < Vente.QuantiteVendu)
            {
                ErrorMessage = "Quantité insuffisante en stock.";
                await OnGetAsync(null, null, null);
                return Page();
            }

            var medicament = await _context.Set<Medication>().FirstOrDefaultAsync(m => m.Id == Vente.MedicamentId);
            if (medicament == null)
            {
                ErrorMessage = "Médicament non trouvé.";
                await OnGetAsync(null, null, null);
                return Page();
            }

            // Vérifiez si PrixBr a une valeur
            if (medicament.PrixBr.HasValue)
            {
                Vente.Montant = medicament.PrixBr.Value * Vente.QuantiteVendu;

                Vente.Gain = Vente.Montant * 0.2m;
            }
            else
            {
                ErrorMessage = "Le prix du médicament est introuvable.";
                await OnGetAsync(null, null, null);
                return Page();
            }

            // Mettre à jour le stock
            stock.Quantite -= Vente.QuantiteVendu;
            Vente.Date = DateTime.Now;

            // Ajouter la vente et mettre à jour le stock
            _context.Set<Vente>().Add(Vente);
            _context.Set<Stock>().Update(stock);

            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostModifier(int id, DateTime date, int medicamentId, int quantiteVendu, decimal montant)
        {
            var vente = await _context.Ventes.FindAsync(id);
            if (vente != null)
            {
                // Retrieve the stock for the specific medication
                var stock = await _context.Stocks.FirstOrDefaultAsync(s => s.MedicamentId == medicamentId);
                if (stock == null)
                {
                    ErrorMessage = "Médicament non trouvé en stock.";
                    await OnGetAsync(null, null, null);// Set error message
                    return Page();  // Return to the same page to display the error
                }

                // Check if the updated quantity is greater than the original quantity
                if (quantiteVendu > vente.QuantiteVendu)
                {
                    // Update stock quantity based on the increased amount
                    int quantityDifference = quantiteVendu - vente.QuantiteVendu;
                    if (stock.Quantite < quantityDifference)
                    {
                        ErrorMessage = "Quantité insuffisante en stock pour cette vente.";
                        await OnGetAsync(null, null, null);// Set error message
                        return Page();  // Return to the same page to display the error
                    }
                    stock.Quantite -= quantityDifference;
                }
                else if (quantiteVendu < vente.QuantiteVendu)
                {
                    // Update stock quantity based on the reduced amount
                    stock.Quantite += (vente.QuantiteVendu - quantiteVendu);
                }

                // Update the sale's details
                vente.Date = date;
                vente.MedicamentId = medicamentId;
                vente.QuantiteVendu = quantiteVendu;
                vente.Montant = montant;
                vente.Gain = vente.Montant * 0.2m;

                // Save changes
                _context.Stocks.Update(stock);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();  // Reload the page to reflect updated data
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var vente = await _context.Set<Vente>().FindAsync(id);
            if (vente == null)
            {
                ErrorMessage = "Vente non trouvée.";
                return Page();
            }

            // Retrieve the stock associated with the sale
            var stock = await _context.Set<Stock>().FirstOrDefaultAsync(s => s.MedicamentId == vente.MedicamentId);
            if (stock != null)
            {
                // Return the quantity back to the stock
                stock.Quantite += vente.QuantiteVendu;
                _context.Set<Stock>().Update(stock);
            }

            // Remove the sale
            _context.Set<Vente>().Remove(vente);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

    }
}