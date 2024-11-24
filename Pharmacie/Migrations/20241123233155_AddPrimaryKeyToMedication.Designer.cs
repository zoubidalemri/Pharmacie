﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pharmacie.Data;

#nullable disable

namespace Pharmacie.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20241123233155_AddPrimaryKeyToMedication")]
    partial class AddPrimaryKeyToMedication
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Pharmacie.Models.Medication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DCI1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Dosage1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Forme")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nom")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PH")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("PPV")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Presentation")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PrincepsGenerique")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("PrixBr")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("TauxRemboursement")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("UniteDosage1")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Medicaments");
                });

            modelBuilder.Entity("Pharmacie.Models.Vente", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Gain")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("MedicamentId")
                        .HasColumnType("int");

                    b.Property<decimal>("Montant")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("MedicamentId");

                    b.ToTable("Ventes");
                });

            modelBuilder.Entity("Pharmacie.Models.Vente", b =>
                {
                    b.HasOne("Pharmacie.Models.Medication", "Medicament")
                        .WithMany()
                        .HasForeignKey("MedicamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Medicament");
                });
#pragma warning restore 612, 618
        }
    }
}
