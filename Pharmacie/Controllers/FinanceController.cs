using Microsoft.AspNetCore.Mvc;
using Pharmacie.Data;

public class FinanceController : Controller
{
    private readonly ApplicationDbContext _context;

    public FinanceController(ApplicationDbContext context)
    {
        _context = context;
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
