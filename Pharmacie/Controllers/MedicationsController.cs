using Microsoft.AspNetCore.Mvc;
using Pharmacie.Services;

public class MedicationsController : Controller
{
    private readonly ExcelService _excelService;

    public MedicationsController(ExcelService excelService)
    {
        _excelService = excelService;
    }

    public async Task<IActionResult> ImportFromExcel()
    {
        try
        {
            await _excelService.GetMedicationsAsync();
            TempData["Message"] = "Les données ont été importées avec succès depuis le fichier Excel.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"Erreur lors de l'importation : {ex.Message}";
        }

        return RedirectToAction("Index");
    }
}
