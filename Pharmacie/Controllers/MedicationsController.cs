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

   
    public IActionResult Autocomplete(string term) {
        if (string.IsNullOrEmpty(term))
    {
        return Json(new string[] { });
    }
   
        var suggestions = _context.Medicaments
           .Where(m => m.Nom.ToLower().StartsWith(term.ToLower()))
          
            .Select(m => m.Nom)
            .ToList();

        return Json(suggestions);
    }



}

