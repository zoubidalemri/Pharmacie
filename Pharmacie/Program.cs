using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Data;
using Pharmacie.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Ajouter la configuration pour la base de donn�es
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (connectionString == null)
    {
        throw new InvalidOperationException("La cha�ne de connexion 'DefaultConnection' est manquante dans le fichier de configuration.");
    }

    options.UseSqlServer(connectionString);
});

// Ajouter les services Razor Pages
builder.Services.AddRazorPages();

// Ajouter le service personnalis� ExcelService
builder.Services.AddSingleton<ExcelService>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var filePath = configuration["ExcelFilePath"];
    var context = sp.GetRequiredService<ApplicationDbContext>();
    return new ExcelService(filePath, context);
});

var app = builder.Build();

// Appeler l'importation automatique
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;

    try
    {
        var excelService = serviceProvider.GetRequiredService<ExcelService>();
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        // Importer les m�dicaments depuis le fichier Excel
        var medications = await excelService.GetMedicationsAsync();

        // V�rifie si les donn�es existent d�j� dans la base
        if (!await dbContext.Medicaments.AnyAsync())
        {
            // Ajouter les m�dicaments import�s
            await dbContext.Medicaments.AddRangeAsync(medications);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("Donn�es import�es depuis le fichier Excel.");
        }
        else
        {
            Console.WriteLine("Les donn�es existent d�j�. Importation ignor�e.");
        }

        // V�rifier si les donn�es ont bien �t� ajout�es � la base de donn�es
        var totalMedicaments = await dbContext.Medicaments.CountAsync();
        Console.WriteLine($"Il y a actuellement {totalMedicaments} m�dicaments dans la base de donn�es.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de l'importation des donn�es : {ex.Message}");
    }
}

// Configurer le pipeline de requ�tes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
