using Microsoft.EntityFrameworkCore;
using Pharmacie.Data;
using Pharmacie.Models;

namespace Pharmacie.Services
{
    public class VenteService
    {
        private readonly ApplicationDbContext _context;

        public VenteService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vente>> GetAllVentesAsync()
        {
            return await _context.Ventes.ToListAsync();
        }

        public async Task AddVenteAsync(Vente vente)
        {
            _context.Ventes.Add(vente);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteVenteAsync(int id)
        {
            var vente = await _context.Ventes.FindAsync(id);
            if (vente != null)
            {
                _context.Ventes.Remove(vente);
                await _context.SaveChangesAsync();
            }
        }
    }

}
