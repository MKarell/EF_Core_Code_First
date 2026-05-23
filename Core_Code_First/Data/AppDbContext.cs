using Microsoft.EntityFrameworkCore;
using EF_Core_Code_First.Models;

namespace EF_Core_Code_First.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<PCs> PCs { get; set; }
        public DbSet<Components> Components { get; set; }
        public DbSet<ComponentTypes> ComponentTypes { get; set; }
        public DbSet<ComponentManufacturers> ComponentManufacturers { get; set; }
        public DbSet<PCComponents> PCComponents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PCs>(e =>
            {
                e.HasKey(p => p.Id);
                e.Property(p => p.Name).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<ComponentTypes>(e =>
            {
                e.HasKey(c => c.Id);
                e.Property(c => c.Abbreviation).HasMaxLength(30);
                e.Property(c => c.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<ComponentManufacturers>(e =>
            {
                e.HasKey(m => m.Id);
                e.Property(m => m.Abbreviation).HasMaxLength(30);
                e.Property(m => m.FullName).HasMaxLength(300);
                e.Property(m => m.FoundationDate).HasColumnType("date");
            });

            modelBuilder.Entity<Components>(e =>
            {
                e.HasKey(c => c.Code);
                e.Property(c => c.Code).HasColumnType("char(10)");
                e.Property(c => c.Name).HasMaxLength(300);

                e.HasOne(c => c.Manufacturers)
                 .WithMany(m => m.Components)
                 .HasForeignKey(c => c.ComponentManufacturersId);

                e.HasOne(c => c.Type)
                 .WithMany(t => t.Components)
                 .HasForeignKey(c => c.ComponentTypesId);
            });

            //[cite_start]// Konfiguracja klucza złożonego dla tabeli łączącej [cite: 58]
            modelBuilder.Entity<PCComponents>(e =>
            {
                e.HasKey(pc => new { pc.PCId, pc.ComponentCode });
                e.Property(pc => pc.ComponentCode).HasColumnType("char(10)");

                e.HasOne(pc => pc.PC)
                 .WithMany(p => p.PCComponents)
                 .HasForeignKey(pc => pc.PCId);

                e.HasOne(pc => pc.Component)
                 .WithMany(c => c.PCComponents)
                 .HasForeignKey(pc => pc.ComponentCode);
            });

            // Seedowanie danych - min. 3 rekordy na tabelę [cite: 61, 62]
            modelBuilder.Entity<ComponentTypes>().HasData(
                new ComponentTypes { Id = 1, Abbreviation = "CPU", Name = "Processor" },
                new ComponentTypes { Id = 2, Abbreviation = "RAM", Name = "Random Access Memory" },
                new ComponentTypes { Id = 3, Abbreviation = "GPU", Name = "Graphics Processing Unit" }
            );

            modelBuilder.Entity<ComponentManufacturers>().HasData(
                new ComponentManufacturers { Id = 1, Abbreviation = "Intel", FullName = "Intel Corporation", FoundationDate = new DateTime(1968, 7, 18) },
                new ComponentManufacturers { Id = 2, Abbreviation = "AMD", FullName = "Advanced Micro Devices", FoundationDate = new DateTime(1969, 5, 1) },
                new ComponentManufacturers { Id = 3, Abbreviation = "Corsair", FullName = "Corsair Gaming, Inc.", FoundationDate = new DateTime(1994, 1, 1) }
            );

            modelBuilder.Entity<Components>().HasData(
                new Components { Code = "COMP000001", Name = "Intel Core i7-12700K", Description = "12-core processor", ComponentManufacturersId = 1, ComponentTypesId = 1 },
                new Components { Code = "COMP000002", Name = "AMD Ryzen 5 5600X", Description = "6-core processor", ComponentManufacturersId = 2, ComponentTypesId = 1 },
                new Components { Code = "COMP000003", Name = "Corsair Vengeance 16GB", Description = "DDR4 3200MHz", ComponentManufacturersId = 3, ComponentTypesId = 2 }
            );

            modelBuilder.Entity<PCs>().HasData(
                new PCs { Id = 1, Name = "Gaming Beast X", Weight = 12.5f, Warranty = 36, CreatedAt = new DateTime(2026, 5, 8, 9, 0, 0), Stock = 5 },
                new PCs { Id = 2, Name = "Office Mini Pro", Weight = 4.2f, Warranty = 24, CreatedAt = new DateTime(2026, 4, 15, 13, 30, 0), Stock = 12 },
                new PCs  { Id = 3, Name = "Home Media Center", Weight = 8.0f, Warranty = 24, CreatedAt = new DateTime(2026, 5, 10, 10, 0, 0), Stock = 3 }
            );

            modelBuilder.Entity<PCComponents>().HasData(
                new PCComponents { PCId = 1, ComponentCode = "COMP000001", Amount = 1 },
                new PCComponents { PCId = 1, ComponentCode = "COMP000003", Amount = 2 },
                new PCComponents { PCId = 2, ComponentCode = "COMP000002", Amount = 1 }
            );
        }
    }
}