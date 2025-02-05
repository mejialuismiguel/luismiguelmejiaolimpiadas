using Microsoft.EntityFrameworkCore;
using AthleteApi.Models;

namespace AthleteApi.Data
{
    public class AthleteContext : DbContext
    {
        public AthleteContext(DbContextOptions<AthleteContext> options) : base(options) { }

        public DbSet<Athlete> Athletes { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración adicional del modelo si es necesario crearlo
            modelBuilder.Entity<Athlete>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
                entity.Property(e => e.BirthDate).IsRequired();
                entity.Property(e => e.Gender).IsRequired().HasMaxLength(1);
                entity.Property(e => e.CountryId).IsRequired();
                entity.Property(e => e.WeightCategoryId).IsRequired();
            });

            modelBuilder.Entity<Tournament>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Location).IsRequired().HasMaxLength(100);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
            });
        }
        
    }
}
