namespace Pharmacie.Services
{
    using OfficeOpenXml;
    using Pharmacie.Models;
    using System.Collections.Generic;
    using System.IO;

    public class ExcelService
    {
        private readonly string _filePath;

        public ExcelService(string filePath)
        {
            _filePath = filePath;
        }

        public List<Medication> GetMedications()
        {
            var medications = new List<Medication>();

            using (var package = new ExcelPackage(new FileInfo(_filePath)))
            {
                var worksheet = package.Workbook.Worksheets[0]; // Get the first worksheet
                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Start from row 2 (skip headers)
                {
                    medications.Add(new Medication
                    {
                        Code = worksheet.Cells[row, 1].Text,
                        Nom = worksheet.Cells[row, 2].Text,
                        DCI1 = worksheet.Cells[row, 3].Text,
                        Dosage1 = worksheet.Cells[row, 4].Text,
                        UniteDosage1 = worksheet.Cells[row, 5].Text,
                        Forme = worksheet.Cells[row, 6].Text,
                        Presentation = worksheet.Cells[row, 7].Text,
                        PPV = decimal.Parse(worksheet.Cells[row, 8].Text),
                        PH = decimal.Parse(worksheet.Cells[row, 9].Text),
                        PrixBr = decimal.Parse(worksheet.Cells[row, 10].Text),
                        PrincepsGenerique = worksheet.Cells[row, 11].Text,
                        TauxRemboursement = decimal.Parse(worksheet.Cells[row, 12].Text)
                    });
                }
            }

            return medications;
        }
    }
}
