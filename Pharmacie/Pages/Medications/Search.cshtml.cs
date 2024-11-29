using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacie.Data;
using Pharmacie.Models;
using System.Collections.Generic;
using System.Linq;

namespace Pharmacie.Pages.Medications
{
    public class SearchModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public SearchModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Medication> SearchResults { get; set; }

        public void OnGet(string query)
        {
            if (string.IsNullOrEmpty(query))
            {
                SearchResults = new List<Medication>();
            }
            else
            {
                SearchResults = _context.Medicaments
                    .Where(m => m.Nom.Contains(query) || m.Presentation.Contains(query))
                    .ToList();
            }
        }
    }
}
