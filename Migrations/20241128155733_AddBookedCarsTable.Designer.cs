﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VeloRent.Models;

#nullable disable

namespace VeloRent.Migrations
{
    [DbContext(typeof(VeloRentContext))]
    [Migration("20241128155733_AddBookedCarsTable")]
    partial class AddBookedCarsTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseCollation("Latin1_General_CI_AS")
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("VeloRent.Models.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarTypeId")
                        .HasColumnType("int");

                    b.Property<bool>("IsAvailable")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("LicensePlate")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("nvarchar(6)");

                    b.Property<int>("LocationId")
                        .HasColumnType("int");

                    b.Property<string>("MaintenanceStatus")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id")
                        .HasName("PK__Car__3214EC07C769F414");

                    b.HasIndex("CarTypeId");

                    b.HasIndex("LocationId");

                    b.HasIndex(new[] { "LicensePlate" }, "UQ__Car__026BC15CAF79E10B")
                        .IsUnique();

                    b.ToTable("Car", (string)null);
                });

            modelBuilder.Entity("VeloRent.Models.CarRentalLocation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__CarRenta__3214EC07049D1820");

                    b.ToTable("CarRentalLocation", (string)null);
                });

            modelBuilder.Entity("VeloRent.Models.CarType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AutomaticOrManual")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<decimal>("DailyRate")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<string>("FuelType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Make")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("NumberOfSeats")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("Id")
                        .HasName("PK__CarType__3214EC07B3F31F0A");

                    b.ToTable("CarType", (string)null);
                });

            modelBuilder.Entity("VeloRent.Models.Customer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ContactNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("DriverLicenseNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id")
                        .HasName("PK__Customer__3214EC07725B05E8");

                    b.HasIndex(new[] { "Username" }, "UQ__Customer__536C85E4480C0BCE")
                        .IsUnique();

                    b.HasIndex(new[] { "Email" }, "UQ__Customer__A9D1053438DFEEA4")
                        .IsUnique();

                    b.HasIndex(new[] { "DriverLicenseNumber" }, "UQ__Customer__C32FF260E1D1CF49")
                        .IsUnique();

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("VeloRent.Models.Rental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<int>("CustomerId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.Property<decimal>("TotalCost")
                        .HasColumnType("decimal(10, 2)");

                    b.HasKey("Id")
                        .HasName("PK__Rental__3214EC07788CFA0A");

                    b.HasIndex("CarId");

                    b.HasIndex("CustomerId");

                    b.ToTable("Rental", (string)null);
                });

            modelBuilder.Entity("VeloRent.Models.Car", b =>
                {
                    b.HasOne("VeloRent.Models.CarType", "CarType")
                        .WithMany("Cars")
                        .HasForeignKey("CarTypeId")
                        .IsRequired()
                        .HasConstraintName("FK__Car__CarTypeId__4316F928");

                    b.HasOne("VeloRent.Models.CarRentalLocation", "Location")
                        .WithMany("Cars")
                        .HasForeignKey("LocationId")
                        .IsRequired()
                        .HasConstraintName("FK__Car__LocationId__440B1D61");

                    b.Navigation("CarType");

                    b.Navigation("Location");
                });

            modelBuilder.Entity("VeloRent.Models.Rental", b =>
                {
                    b.HasOne("VeloRent.Models.Car", "Car")
                        .WithMany("Rentals")
                        .HasForeignKey("CarId")
                        .IsRequired()
                        .HasConstraintName("FK__Rental__CarId__44FF419A");

                    b.HasOne("VeloRent.Models.Customer", "Customer")
                        .WithMany("Rentals")
                        .HasForeignKey("CustomerId")
                        .IsRequired()
                        .HasConstraintName("FK__Rental__Customer__45F365D3");

                    b.Navigation("Car");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("VeloRent.Models.Car", b =>
                {
                    b.Navigation("Rentals");
                });

            modelBuilder.Entity("VeloRent.Models.CarRentalLocation", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("VeloRent.Models.CarType", b =>
                {
                    b.Navigation("Cars");
                });

            modelBuilder.Entity("VeloRent.Models.Customer", b =>
                {
                    b.Navigation("Rentals");
                });
#pragma warning restore 612, 618
        }
    }
}
