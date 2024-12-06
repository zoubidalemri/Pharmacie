using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Models;

namespace Pharmacie.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StocksController : ControllerBase
    {
        private readonly DbContext _context;

        public StocksController(DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Stock>>> GetStocks()
        {
            return await _context.Set<Stock>().Include(s => s.Medicament).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Stock>> AddStock(Stock stock)
        {
            _context.Set<Stock>().Add(stock);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStocks), new { id = stock.Id }, stock);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStock(int id, Stock stock)
        {
            if (id != stock.Id)
                return BadRequest();

            _context.Entry(stock).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StockExists(id))
                    return NotFound();

                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStock(int id)
        {
            var stock = await _context.Set<Stock>().FindAsync(id);

            if (stock == null)
                return NotFound();

            _context.Set<Stock>().Remove(stock);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StockExists(int id)
        {
            return _context.Set<Stock>().Any(e => e.Id == id);
        }
    }
}
