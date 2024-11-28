using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace VeloRent.Models;

public partial class VeloRentContext : DbContext
{
    public VeloRentContext()
    {
    }

    public VeloRentContext(DbContextOptions<VeloRentContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BookedCar> BookedCars { get; set; }

    public virtual DbSet<Car> Cars { get; set; }

    public virtual DbSet<CarRentalLocation> CarRentalLocations { get; set; }

    public virtual DbSet<CarType> CarTypes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Rental> Rentals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source = localhost; Initial Catalog = VeloRent; Integrated Security = True; Trust Server Certificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Latin1_General_CI_AS");

        modelBuilder.Entity<BookedCar>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BookedCa__3214EC07D253D97A");

            entity.HasOne(d => d.Car).WithMany(p => p.BookedCars)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookedCar__CarId__52593CB8");

            entity.HasOne(d => d.Rental).WithMany(p => p.BookedCars)
                .HasForeignKey(d => d.RentalId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BookedCar__Renta__534D60F1");
        });

        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Car__3214EC07C769F414");

            entity.ToTable("Car");

            entity.HasIndex(e => e.LicensePlate, "UQ__Car__026BC15CAF79E10B").IsUnique();

            entity.Property(e => e.IsAvailable).HasDefaultValue(true);
            entity.Property(e => e.LicensePlate).HasMaxLength(6);
            entity.Property(e => e.MaintenanceStatus).HasMaxLength(50);
            entity.Property(e => e.Status).HasMaxLength(30);

            entity.HasOne(d => d.CarType).WithMany(p => p.Cars)
                .HasForeignKey(d => d.CarTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__CarTypeId__4316F928");

            entity.HasOne(d => d.Location).WithMany(p => p.Cars)
                .HasForeignKey(d => d.LocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Car__LocationId__440B1D61");
        });

        modelBuilder.Entity<CarRentalLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarRenta__3214EC07049D1820");

            entity.ToTable("CarRentalLocation");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
        });

        modelBuilder.Entity<CarType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CarType__3214EC07B3F31F0A");

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
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07725B05E8");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Username, "UQ__Customer__536C85E4480C0BCE").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D1053438DFEEA4").IsUnique();

            entity.HasIndex(e => e.DriverLicenseNumber, "UQ__Customer__C32FF260E1D1CF49").IsUnique();

            entity.Property(e => e.ContactNumber).HasMaxLength(50);
            entity.Property(e => e.DriverLicenseNumber).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Rental__3214EC07788CFA0A");

            entity.ToTable("Rental");

            entity.Property(e => e.TotalCost).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Car).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.CarId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rental__CarId__44FF419A");

            entity.HasOne(d => d.Customer).WithMany(p => p.Rentals)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Rental__Customer__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
