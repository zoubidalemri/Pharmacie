using Microsoft.AspNetCore.Mvc;
using Pharmacie.Services;

public class MedicationsController : Controller
{
    private readonly ExcelService _excelService;

    public MedicationsController(ExcelService excelService)
    {
        _excelService = excelService;
    }

    public IActionResult Index()
    {
        var medications = _excelService.GetMedications();
        return View(medications);
    }
}
