using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Data; // Adaptez selon l'emplacement de votre DbContext
using Pharmacie.Models; // Adaptez selon l'emplacement de vos modèles

namespace Pharmacie.Pages.Finance
{
    public class IndexfModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public decimal TotalGains { get; set; }         // Gains totaux
        public int TotalVentes { get; set; }           // Nombre total de ventes
        public List<Vente> DernieresVentes { get; set; } // Liste des dernières ventes

        public IndexfModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            // Récupérer toutes les ventes
            var ventes = await _context.Ventes
                .Include(v => v.Medicament) // Charger les médicaments associés
                .ToListAsync();

            TotalGains = ventes.Sum(v => v.Gain);       // Calculer les gains totaux
            TotalVentes = ventes.Count;                // Compter le nombre de ventes
            DernieresVentes = ventes.OrderByDescending(v => v.Date) // Les dernières ventes
                                     .Take(10)          // Limité à 10 ventes
                                     .ToList();
        }
    }
}
