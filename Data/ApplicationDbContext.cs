using Microsoft.EntityFrameworkCore;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Data;

/// <summary>
/// Contexto de base de datos para la aplicación Million Real Estate
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Owner> Owners { get; set; }
    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<PropertyTrace> PropertyTraces { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Owner>(entity =>
        {
            entity.HasKey(e => e.IdOwner);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Photo).HasMaxLength(500);
            entity.Property(e => e.Birthday).HasColumnType("date");
        });

        modelBuilder.Entity<Property>(entity =>
        {
            entity.HasKey(e => e.IdProperty);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Address).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.CodeInternal).IsRequired().HasMaxLength(50);

            entity.HasOne(e => e.Owner)
                .WithMany(o => o.Properties)
                .HasForeignKey(e => e.IdOwner)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(e => e.IdPropertyImage);
            entity.Property(e => e.File).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Enabled).HasDefaultValue(true);

            entity.HasOne(e => e.Property)
                .WithMany(p => p.PropertyImages)
                .HasForeignKey(e => e.IdProperty)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PropertyTrace>(entity =>
        {
            entity.HasKey(e => e.IdPropertyTrace);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Value).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Tax).HasColumnType("decimal(18,2)");

            entity.HasOne(e => e.Property)
                .WithMany(p => p.PropertyTraces)
                .HasForeignKey(e => e.IdProperty)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}