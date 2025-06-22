using HouseBroker.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Data;

public class HouseBrokerDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public HouseBrokerDbContext(DbContextOptions<HouseBrokerDbContext> options) : base(options)
    {
    }

    public DbSet<Property> Properties { get; set; }
    public DbSet<PropertyImage> PropertyImages { get; set; }
    public DbSet<PropertyFeature> PropertyFeatures { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // User configuration
        builder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(u => u.LastName).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Role).IsRequired();
            entity.Property(u => u.CreatedAt).IsRequired();
            entity.Property(u => u.IsActive).IsRequired().HasDefaultValue(true);
            
            entity.HasMany(u => u.Properties)
                .WithOne(p => p.Broker)
                .HasForeignKey(p => p.BrokerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Property configuration
        builder.Entity<Property>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Title).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).IsRequired().HasMaxLength(2000);
            entity.Property(p => p.PropertyType).IsRequired();
            entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(p => p.Address).IsRequired().HasMaxLength(300);
            entity.Property(p => p.City).IsRequired().HasMaxLength(100);
            entity.Property(p => p.State).IsRequired().HasMaxLength(100);
            entity.Property(p => p.ZipCode).IsRequired().HasMaxLength(20);
            entity.Property(p => p.Country).IsRequired().HasMaxLength(100);
            entity.Property(p => p.Latitude).IsRequired();
            entity.Property(p => p.Longitude).IsRequired();
            entity.Property(p => p.Bedrooms).IsRequired();
            entity.Property(p => p.Bathrooms).IsRequired();
            entity.Property(p => p.SquareFeet).IsRequired();
            entity.Property(p => p.YearBuilt).IsRequired();
            entity.Property(p => p.IsAvailable).IsRequired().HasDefaultValue(true);
            entity.Property(p => p.CreatedAt).IsRequired();

            entity.HasMany(p => p.Images)
                .WithOne(i => i.Property)
                .HasForeignKey(i => i.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(p => p.Features)
                .WithOne(f => f.Property)
                .HasForeignKey(f => f.PropertyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Indexes for better query performance
            entity.HasIndex(p => p.City);
            entity.HasIndex(p => p.State);
            entity.HasIndex(p => p.PropertyType);
            entity.HasIndex(p => p.Price);
            entity.HasIndex(p => p.IsAvailable);
            entity.HasIndex(p => p.BrokerId);
        });

        // PropertyImage configuration
        builder.Entity<PropertyImage>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ImageUrl).IsRequired().HasMaxLength(500);
            entity.Property(i => i.Description).HasMaxLength(200);
            entity.Property(i => i.IsPrimary).IsRequired().HasDefaultValue(false);
            entity.Property(i => i.DisplayOrder).IsRequired().HasDefaultValue(1);
            entity.Property(i => i.CreatedAt).IsRequired();
        });

        // PropertyFeature configuration
        builder.Entity<PropertyFeature>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Name).IsRequired().HasMaxLength(100);
            entity.Property(f => f.Description).HasMaxLength(500);
            entity.Property(f => f.CreatedAt).IsRequired();
        });
    }
}