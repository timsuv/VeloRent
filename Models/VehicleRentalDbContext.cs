using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VeloRent.Models;

public partial class VehicleRentalDbContext : DbContext
{
    public VehicleRentalDbContext()
    {
    }

    public VehicleRentalDbContext(DbContextOptions<VehicleRentalDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarRentalLocation> CarRentalLocations { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=VehicleRentalDB;Integrated Security=True;Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Car__3214EC07671F9C02");

            entity.ToTable("Car");

            entity.HasIndex(e => e.LicensePlate, "UQ__Car__026BC15CCBCDE4E1").IsUnique();

            entity.Property(e => e.LicensePlate).HasMaxLength(6);
            entity.Property(e => e.MaintenanceStatus).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.CarType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__CarTypeId__3F466844");

            entity.HasOne(d => d.Location).WithMany(p => p.Cars)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__LocationId__403A8C7D");
        });

        modelBuilder.Entity<CarRentalLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarRenta__3214EC07EBC677D6");

            entity.ToTable("CarRentalLocation");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarType__3214EC079D64FA84");

            entity.ToTable("CarType");

            entity.Property(e => e.AutomaticOrManual).HasMaxLength(10);
            entity.Property(e => e.DailyRate).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.FuelType).HasMaxLength(20);
            entity.Property(e => e.Make).HasMaxLength(50);
            entity.Property(e => e.Model).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(20);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0730852E42");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Username, "UQ__Customer__536C85E46ACED2F8").IsUnique();

            entity.HasIndex(e => e.DriverLicenseNumber, "UQ__Customer__C32FF2601948D820").IsUnique();

            entity.Property(e => e.ContactNumber).HasMaxLength(50);
            entity.Property(e => e.DriverLicenseNumber).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rental__3214EC07CC0BE766");

            entity.ToTable("Rental");

            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Car).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rental__CarId__45F365D3");

            entity.HasOne(d => d.Customer).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rental__Customer__46E78A0C");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
