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
                .ToTable("Medicaments","dbo")
                .HasKey(m => m.Id);  // Spécifie que 'Id' est la clé primaire

            modelBuilder.Entity<Vente>(entity =>
            {
                // Préciser la précision et l'échelle pour 'Gain'
                entity.Property(v => v.Gain)
                      .HasPrecision(18, 4); // 18 chiffres au total, 4 après la virgule

                // Préciser la précision et l'échelle pour 'Montant'
                entity.Property(v => v.Montant)
                      .HasPrecision(18, 2); // 18 chiffres au total, 2 après la virgule
            });
            modelBuilder.Entity<Medication>(entity =>
            {
                entity.Property(m => m.PH)
                      .HasPrecision(18, 2); // Ajustez selon vos besoins
                entity.Property(m => m.PPV)
                      .HasPrecision(18, 2); // Ajustez selon vos besoins
                entity.Property(m => m.PrixBr)
                      .HasPrecision(18, 2); // Ajustez selon vos besoins
                entity.Property(m => m.TauxRemboursement)
                      .HasPrecision(5, 2); // Par exemple, pour un taux comme 99.99 %
            });
          

        }

    }
}
