using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Data;
using Pharmacie.Models;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Pharmacie.Pages.Stocks
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }
     
     


        // Properties to hold the list of stocks and medications
        public List<Stock> Stocks { get; set; }
        public List<Medication> Medications { get; set; }

        // OnGetAsync method to fetch data when the page loads
        public async Task OnGetAsync()
        {
            // Fetch all the stocks from the database, including the related Medicament entities
            Stocks = await _context.Stocks.Include(s => s.Medicament).ToListAsync();

            // Fetch the list of medications (Medicaments) for the dropdown or other purposes
            Medications = await _context.Medicaments.ToListAsync();
        }

        // OnPostAsync method to handle form submissions for adding new stock
        public async Task<IActionResult> OnPostAsync(int medicamentId, int quantity)
        {
            // Find the selected Medicament from the database
            var medicament = await _context.Medicaments.FindAsync(medicamentId);
            if (medicament == null)
            {
                // If Medicament doesn't exist, return with an error message
                ModelState.AddModelError(string.Empty, "Medicament not found.");
                return Page();
            }

            // Create a new Stock entry
            var stock = new Stock
            {
                MedicamentId = medicamentId,
                Quantite = quantity,
                Medicament = medicament // Establish the relationship with the Medicament
            };

            // Add the new Stock to the database
            _context.Stocks.Add(stock);
            await _context.SaveChangesAsync();

            // Redirect to the same page to refresh the data
            return RedirectToPage();
        }

        // OnPostDeleteAsync method to delete a stock entry by its ID
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            // Find the stock to be deleted
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                _context.Stocks.Remove(stock);
                await _context.SaveChangesAsync();
            }

            // Redirect to the same page to refresh the data
            return RedirectToPage();
        }

        // OnPostUpdateAsync method to update the stock quantity (if needed)
        public async Task<IActionResult> OnPostUpdateAsync(int id, int quantity)
        {
            // Find the stock entry to update
            var stock = await _context.Stocks.FindAsync(id);
            if (stock != null)
            {
                stock.Quantite = quantity;
                await _context.SaveChangesAsync();
            }

            // Redirect to the same page to refresh the data
            return RedirectToPage();
        }
    }
}
