using OfficeOpenXml;
using Pharmacie.Data;
using Pharmacie.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pharmacie.Services
{
    public class ExcelService
    {
        private readonly string _filePath;
        private readonly ApplicationDbContext _context;

        // Modifiez le constructeur pour accepter deux paramètres
        public ExcelService(string filePath, ApplicationDbContext context)
        {
            _filePath = filePath;
            _context = context;
        }

        public async Task<List<Medication>> GetMedicationsAsync()
        {
            var medications = new List<Medication>();
            var fileInfo = new FileInfo(_filePath);

            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("Fichier Excel introuvable à l'emplacement spécifié.", _filePath);
            }

            // Configurer EPPlus
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var package = new ExcelPackage(fileInfo))
            {
                await Task.Run(() =>
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
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
                });
            }

            return medications;
        }
    }
}
