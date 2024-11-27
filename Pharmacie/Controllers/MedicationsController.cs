using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Data;
using Pharmacie.Models;

    public class MedicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MedicationsController(ApplicationDbContext context)
        {
            _context = context;
        }
    public IActionResult Indexx()
    {
        var medicaments = _context.Medicaments.ToList();
        return View(medicaments);
    }

      
    }

