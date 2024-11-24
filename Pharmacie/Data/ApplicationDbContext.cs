using Microsoft.EntityFrameworkCore;
using Pharmacie.Models;  // Assurez-vous que les modèles comme Vente et Medication sont bien importés

namespace Pharmacie.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Medication> Medicaments { get; set; }  // Table des médicaments
        public DbSet<Vente> Ventes { get; set; }            // Table des ventes

        // Cette méthode est appelée lors de la création du modèle
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure la clé primaire pour l'entité Medication
            modelBuilder.Entity<Medication>()
                .HasKey(m => m.Id);  // Spécifie que 'Id' est la clé primaire

        }

    }
}
