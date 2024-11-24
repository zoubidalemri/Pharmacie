using OfficeOpenXml;
using System.IO;
using System.Collections.Generic;
using Pharmacie.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pharmacie.Pages.Medications
{
    public class IndexxModel : PageModel
    {
        public List<Medication> Medications { get; set; } = new List<Medication>();

        public void OnGet()
        {
            // Load data from the Excel file
            Medications = LoadMedicationsFromExcel("C:\\Users\\info\\Downloads\\ref-des-medicaments-cnops-2014.xlsx");

        }

        private List<Medication> LoadMedicationsFromExcel(string filePath)
        {
            var medications = new List<Medication>();

            // Ensure that EPPlus is used correctly by enabling license context
            ExcelPackage.LicenseContext = LicenseContext.Commercial;

            using (var package = new ExcelPackage(new FileInfo(filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Get the first sheet
                var rowCount = worksheet.Dimension.Rows; // Get row count
                var colCount = worksheet.Dimension.Columns; // Get column count

                // Loop through each row (starting from row 2, assuming row 1 is header)
                for (int row = 2; row <= rowCount; row++)
                {
                    var medication = new Medication
                    {
                        Code = worksheet.Cells[row, 1].Text,
                        Nom = worksheet.Cells[row, 2].Text,
                        DCI1 = worksheet.Cells[row, 3].Text,
                        Dosage1 = worksheet.Cells[row, 4].Text,
                        UniteDosage1 = worksheet.Cells[row, 5].Text,
                        Forme = worksheet.Cells[row, 6].Text,
                        Presentation = worksheet.Cells[row, 7].Text,
                        PPV = decimal.TryParse(worksheet.Cells[row, 8].Text, out decimal ppv) ? ppv : 0,
                        PH = decimal.TryParse(worksheet.Cells[row, 9].Text, out decimal ph) ? ph : 0,
                        PrixBr = decimal.TryParse(worksheet.Cells[row, 10].Text, out decimal prixBr) ? prixBr : 0,
                        PrincepsGenerique = worksheet.Cells[row, 11].Text,
                        TauxRemboursement = decimal.TryParse(worksheet.Cells[row, 12].Text, out decimal tauxRemboursement) ? tauxRemboursement : 0
                    };

                    medications.Add(medication);
                }
            }

            return medications;
        }
    }
}
