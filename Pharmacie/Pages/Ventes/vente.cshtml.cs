using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Models;
using Pharmacie.Data;

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

        public async Task OnGetAsync()
        {
            Ventes = await _context.Set<Vente>().Include(v => v.Medicament).ToListAsync();
            Medications = await _context.Set<Medication>().ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ErrorMessage = "Données invalides.";
                await OnGetAsync();
                return Page();
            }

            var stock = await _context.Set<Stock>().FirstOrDefaultAsync(s => s.MedicamentId == Vente.MedicamentId);

            if (stock == null )
            {
                ErrorMessage = "Medicament non trouvé en stock.";
                await OnGetAsync();
                return Page();
            }
            if (stock.Quantite < Vente.QuantiteVendu)
            {
                ErrorMessage = "Quantité insuffisante en stock.";
                await OnGetAsync();
                return Page();
            }
            stock.Quantite -= Vente.QuantiteVendu;
            Vente.Date = DateTime.Now;

            _context.Set<Vente>().Add(Vente);
            _context.Set<Stock>().Update(stock);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
