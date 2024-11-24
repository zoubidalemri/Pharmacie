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

// Ajouter la configuration pour la base de données
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (connectionString == null)
    {
        throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' est manquante dans le fichier de configuration.");
    }

    options.UseSqlServer(connectionString);
});

// Ajouter les services Razor Pages
builder.Services.AddRazorPages();

// Ajouter le service personnalisé ExcelService
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

        // Importer les médicaments depuis le fichier Excel
        var medications = await excelService.GetMedicationsAsync();

        // Vérifie si les données existent déjà dans la base
        if (!await dbContext.Medicaments.AnyAsync())
        {
            // Ajouter les médicaments importés
            await dbContext.Medicaments.AddRangeAsync(medications);
            await dbContext.SaveChangesAsync();
            Console.WriteLine("Données importées depuis le fichier Excel.");
        }
        else
        {
            Console.WriteLine("Les données existent déjà. Importation ignorée.");
        }

        // Vérifier si les données ont bien été ajoutées à la base de données
        var totalMedicaments = await dbContext.Medicaments.CountAsync();
        Console.WriteLine($"Il y a actuellement {totalMedicaments} médicaments dans la base de données.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Erreur lors de l'importation des données : {ex.Message}");
    }
}

// Configurer le pipeline de requêtes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
