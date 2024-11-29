using Pharmacie.Pages.Ventes; // Ensure the correct namespace is included
using Microsoft.AspNetCore.Mvc;
using Pharmacie.Data;

// Adjust if needed to include your context

namespace Pharmacie.Controllers
{
    public class FinanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FinanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Action for viewing the historical sales page (Historique)
       

        // Action for deleting a sale
        [HttpPost]
        public IActionResult DeleteVente(int deletedId, string returnUrl)
        {
            var vente = _context.Ventes.Find(deletedId);
            if (vente != null)
            {
                _context.Ventes.Remove(vente);
                _context.SaveChanges();
                TempData["Message"] = "Vente supprimée avec succès.";
            }
            else
            {
                TempData["Message"] = "La vente n'a pas été trouvée.";
            }

            // Redirect back to the same page (Historique)
            return RedirectToAction("Historique");
        }




        public IActionResult Index()
        {
            var totalVentes = _context.Ventes.Sum(v => v.Montant);
            var totalNombreVentes = _context.Ventes.Count();

            // Statistiques par mois ou par année (exemple simple)
            var ventesParMois = _context.Ventes
                .GroupBy(v => v.Date.Month)
                .Select(g => new { Mois = g.Key, Total = g.Sum(v => v.Montant) })
                .ToList();

            // Passer les données à la vue
            ViewBag.TotalVentes = totalVentes;
            ViewBag.TotalNombreVentes = totalNombreVentes;
            ViewBag.VentesParMois = ventesParMois;

            return View();
        }



    }
}
