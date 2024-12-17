using Microsoft.AspNetCore.Mvc.RazorPages;
using Pharmacie.Data;
using Pharmacie.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

using Microsoft.AspNetCore.Mvc;

namespace Pharmacie.Pages.Finance
{
    public class VenteParMois
    {
        public string Mois { get; set; }
        public decimal Montant { get; set; }
        public decimal Gain { get; set; }
    }

    public class IndexfModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<VenteParMois> VentesParMois { get; set; }
        public decimal TotalVentes { get; set; }
        public decimal TotalGains { get; set; }

        [BindProperty]
        public string TypeStatistique { get; set; } // "mois" ou "annee"
        [BindProperty]
        public int? Annee { get; set; } // Année saisie par l'utilisateur
        [BindProperty]
        public string MoisAnnee { get; set; } // Format "MM/yyyy"

        public IndexfModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnPostAsync()
        {
            // Vérification de la statistique demandée et traitement
            if (TypeStatistique == "mois" && !string.IsNullOrEmpty(MoisAnnee))
            {
                var parts = MoisAnnee.Split('/');
                int mois = int.Parse(parts[0]);
                int annee = int.Parse(parts[1]);

                // Récupérer les ventes et gains par jour dans un mois donné
                var ventes = await _context.Ventes
                    .Where(v => v.Date.Year == annee && v.Date.Month == mois)
                    .GroupBy(v => v.Date.Day)
                    .Select(g => new
                    {
                        Jour = g.Key,
                        Montant = g.Sum(e => e.Montant),
                        Gain = g.Sum(e => e.Gain)
                    })
                    .ToListAsync();

                // Convertir les résultats pour les afficher dans le modèle
                VentesParMois = ventes.Select(v => new VenteParMois
                {
                    Mois = $"Jour {v.Jour}",
                    Montant = v.Montant,
                    Gain = v.Gain
                }).ToList();
            }
            else if (TypeStatistique == "annee" && Annee.HasValue)
            {
                // Récupérer les ventes et gains par mois dans une année donnée
                var ventes = await _context.Ventes
                    .Where(v => v.Date.Year == Annee.Value)
                    .GroupBy(v => v.Date.Month)
                    .Select(g => new
                    {
                        Mois = g.Key,
                        Montant = g.Sum(e => e.Montant),
                        Gain = g.Sum(e => e.Gain)
                    })
                    .ToListAsync();

                // Convertir les résultats pour les afficher dans le modèle
                VentesParMois = ventes.Select(v => new VenteParMois
                {
                    Mois = $"{v.Mois}/{Annee.Value}",
                    Montant = v.Montant,
                    Gain = v.Gain
                }).ToList();
            }

            // Calculer les totaux des ventes et gains
            TotalVentes = VentesParMois.Sum(v => v.Montant);
            TotalGains = VentesParMois.Sum(v => v.Gain);
        }

        public void OnGet()
        {
            // Initialisation
            VentesParMois = new List<VenteParMois>();
        }
    }
}
