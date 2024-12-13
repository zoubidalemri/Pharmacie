using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Pharmacie.Data;

using System;
using System.Linq;
using System.Threading.Tasks;
using Pharmacie.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    if (connectionString == null)
    {
        throw new InvalidOperationException("La chaîne de connexion 'DefaultConnection' est manquante dans le fichier de configuration.");
    }

    options.UseSqlServer(connectionString);
   
});


builder.Services.AddRazorPages();

builder.Services.AddScoped<VenteService>();
builder.Services.AddControllersWithViews();


var app = builder.Build();






// Configurer le pipeline de requêtes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Vente}/{action=vente}/{id?}");


app.Run();




