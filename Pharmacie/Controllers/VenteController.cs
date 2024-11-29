using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Data;
using Pharmacie.Models;
using Pharmacie.Services;

[Route("Vente")]
public class VenteController : Controller
{
    private readonly VenteService _venteService;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<VenteController> _logger;

    public List<Medication> Medications { get; private set; }
    public List<Vente> ventes { get; private set; }

    public VenteController(VenteService venteService, ApplicationDbContext context, ILogger<VenteController> logger)
    {
        _venteService = venteService;
        _context = context;
        _logger = logger;
    }

    public async Task<IActionResult> GestionDesVentes()
    {
        var ventes = await _context.Ventes.Include(v => v.Medicament).ToListAsync();
    var medications = await _context.Medicaments.ToListAsync();
    ViewBag.Ventes = ventes;
    ViewBag.Medications = medications;
        // Correct: Passing a List<Vente>
        return View(ventes);


    }

    [HttpPost]
    public async Task<IActionResult> AddVente(int MedicamentId, int QuantiteVendu)
    {
        var medicament = await _context.Medicaments.FindAsync(MedicamentId);
        if (medicament == null)
        {
            // Handle error if medicament is not found
            ModelState.AddModelError(string.Empty, "Médicament introuvable.");
            return RedirectToAction("/Ventes/vente");
        }

        var vente = new Vente
        {
            MedicamentId = MedicamentId,
            QuantiteVendu = QuantiteVendu,
            Montant = (decimal)((medicament.PrixBr??0) * QuantiteVendu),
            Gain = (decimal)((medicament.PrixBr ?? 0) * QuantiteVendu)
        };

        _context.Ventes.Add(vente);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(vente));
    }

    
}

