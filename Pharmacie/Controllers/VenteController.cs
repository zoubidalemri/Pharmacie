using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Models;

namespace Pharmacie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VentesController : ControllerBase
    {
        private readonly DbContext _context;

        public VentesController(DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vente>>> GetVentes()
        {
            return await _context.Set<Vente>().Include(v => v.Medicament).ToListAsync();
        }

        [HttpPost]
        public async Task<IActionResult> AddVente(Vente vente)
        {
            var stock = await _context.Set<Stock>().FirstOrDefaultAsync(s => s.MedicamentId == vente.MedicamentId);

            if (stock == null || stock.Quantite < vente.QuantiteVendu)
                return BadRequest("Insufficient stock for this sale.");

            stock.Quantite -= vente.QuantiteVendu;

            _context.Set<Vente>().Add(vente);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }

            return CreatedAtAction(nameof(GetVentes), new { id = vente.Id }, vente);
        }
    }
}
