using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace RentACarDB.Models;

public partial class RentacardbContext : DbContext
{
    public RentacardbContext() { }

    public RentacardbContext(DbContextOptions<RentacardbContext> options)
        : base(options) { }

    public virtual DbSet<Car> Cars { get; set; }
    public virtual DbSet<City> Cities { get; set; }
    public virtual DbSet<Customer> Customers { get; set; }
    public virtual DbSet<District> Districts { get; set; }
    public virtual DbSet<Rental> Rentals { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
#warning "Bağlantı bilgisini gizli tutun!"
        optionsBuilder.UseMySql("server=localhost;database=rentacardb;user=root;password=14320998",
            Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.42-mysql"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("utf8mb4_0900_ai_ci").HasCharSet("utf8mb4");

        // Car
        modelBuilder.Entity<Car>(entity =>
        {
            entity.HasKey(e => e.CarsId).HasName("PRIMARY");
            entity.ToTable("cars");

            entity.Property(e => e.CarsId).HasColumnName("carsID");
            entity.Property(e => e.Brand).HasMaxLength(50).HasColumnName("brand");
            entity.Property(e => e.Model).HasMaxLength(50).HasColumnName("model");
            entity.Property(e => e.Plate).HasMaxLength(50).HasColumnName("plate");
            entity.Property(e => e.ImageName).HasMaxLength(50);
            entity.Property(e => e.DailyPrice).HasMaxLength(50).HasColumnName("dailyPrice");
            entity.Property(e => e.RentCount).HasColumnName("rentCount");
            entity.Property(e => e.Year).HasColumnType("year").HasColumnName("year");

            entity.Property(e => e.DistrictId).HasColumnName("districtID");

            entity.HasOne(d => d.District)
                .WithMany(p => p.Cars)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_district");
        });

        // City
        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("cities");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("NAME");
        });

        // Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");
            entity.ToTable("customers");

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.CustomerName).HasMaxLength(50);
            entity.Property(e => e.CustomerLastName).HasMaxLength(50);
            entity.Property(e => e.CustomerEmail).HasMaxLength(50).HasColumnName("CustomerEMail");
            entity.Property(e => e.CustomerPassword).HasMaxLength(50);
            entity.Property(e => e.CustomerTellNo).HasMaxLength(50);
            entity.Property(e => e.Cities).HasColumnName("cities");
            entity.Property(e => e.District).HasColumnName("district");

            entity.HasOne(d => d.CitiesNavigation)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.Cities)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("customers_ibfk_1");

            entity.HasOne(d => d.DistrictNavigation)
                .WithMany(p => p.Customers)
                .HasForeignKey(d => d.District)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("customers_ibfk_2");
        });

        // District
        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("district");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(50).HasColumnName("NAME");
            entity.Property(e => e.CityId).HasColumnName("CityId");

            entity.HasOne(d => d.City)
                .WithMany(p => p.Districts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_district_cities");
        });

        // Rental
        modelBuilder.Entity<Rental>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");
            entity.ToTable("rental");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.CarsId).HasColumnName("carsID");
            entity.Property(e => e.CustomerId).HasColumnName("customerID");
            entity.Property(e => e.StartDate).HasColumnType("date").HasColumnName("startDate");
            entity.Property(e => e.EndDate).HasColumnType("date").HasColumnName("endDate");
            entity.Property(e => e.TotalPrice).HasColumnName("totalPrice");
            entity.Property(e => e.Avaible).HasColumnType("bool");

            entity.HasOne(d => d.Cars)
                .WithMany(p => p.Rentals)
                .HasForeignKey(d => d.CarsId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_cars");

            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Rentals)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_customers");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
