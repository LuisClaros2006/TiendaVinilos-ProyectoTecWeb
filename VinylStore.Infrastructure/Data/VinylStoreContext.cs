using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VinylStore.Core.Entities;
// using VinylStore.Infrastructure.Entities;

namespace VinylStore.Infrastructure.Data;

public partial class VinylStoreContext : DbContext
{
    public VinylStoreContext()
    {

    }

    public VinylStoreContext(DbContextOptions<VinylStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Saledetail> Saledetails { get; set; }

    public virtual DbSet<Vinyl> Vinyls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Album>(entity =>
        {
            entity.HasKey(e => e.AlbumId).HasName("PRIMARY");

            entity.ToTable("album");

            entity.HasIndex(e => e.ArtistId, "IX_Album_ArtistId");

            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Artist).WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Album_Artist");
        });

        modelBuilder.Entity<Artist>(entity =>
        {
            entity.HasKey(e => e.ArtistId).HasName("PRIMARY");

            entity.ToTable("artist");

            entity.HasIndex(e => e.Name, "UQ_Artist_Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PRIMARY");

            entity.ToTable("customer");

            entity.Property(e => e.Address).HasMaxLength(250);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(80);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.LastName).HasMaxLength(80);
            entity.Property(e => e.Phone).HasMaxLength(30);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PRIMARY");

            entity.ToTable("employee");

            entity.HasIndex(e => e.Email, "UQ_Employee_Email").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FirstName).HasMaxLength(80);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.LastName).HasMaxLength(80);
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'EMPLOYEE'")
                .HasColumnType("enum('ADMIN','EMPLOYEE')");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PRIMARY");

            entity.ToTable("genre");

            entity.HasIndex(e => e.Name, "UQ_Genre_Name").IsUnique();

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.HasKey(e => e.SaleId).HasName("PRIMARY");

            entity.ToTable("sale");

            entity.HasIndex(e => e.CustomerId, "IX_Sale_CustomerId");

            entity.HasIndex(e => e.EmployeeId, "IX_Sale_EmployeeId");

            entity.HasIndex(e => e.SaleDate, "IX_Sale_SaleDate");

            entity.Property(e => e.CancelledAt).HasColumnType("datetime");
            entity.Property(e => e.SaleDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'COMPLETED'")
                .HasColumnType("enum('COMPLETED','CANCELLED')");
            entity.Property(e => e.Total).HasPrecision(12, 2);

            entity.HasOne(d => d.Customer).WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Customer");

            entity.HasOne(d => d.Employee).WithMany(p => p.Sales)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Employee");
        });

        modelBuilder.Entity<Saledetail>(entity =>
        {
            entity.HasKey(e => e.SaleDetailId).HasName("PRIMARY");

            entity.ToTable("saledetail");

            entity.HasIndex(e => e.VinylId, "IX_SaleDetail_VinylId");

            entity.HasIndex(e => new { e.SaleId, e.VinylId }, "UQ_SaleDetail_Sale_Vinyl").IsUnique();

            entity.Property(e => e.UnitPrice).HasPrecision(10, 2);

            entity.HasOne(d => d.Sale).WithMany(p => p.Saledetails)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetail_Sale");

            entity.HasOne(d => d.Vinyl).WithMany(p => p.Saledetails)
                .HasForeignKey(d => d.VinylId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetail_Vinyl");
        });

        modelBuilder.Entity<Vinyl>(entity =>
        {
            entity.HasKey(e => e.VinylId).HasName("PRIMARY");

            entity.ToTable("vinyl");

            entity.HasIndex(e => e.AlbumId, "IX_Vinyl_AlbumId");

            entity.HasIndex(e => e.GenreId, "IX_Vinyl_GenreId");

            entity.HasIndex(e => e.Sku, "UQ_Vinyl_Sku").IsUnique();

            entity.Property(e => e.CoverImageUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Edition).HasMaxLength(120);
            entity.Property(e => e.Format)
                .HasDefaultValueSql("'LP'")
                .HasColumnType("enum('LP','EP','SINGLE')");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.Property(e => e.Sku).HasMaxLength(40);

            entity.HasOne(d => d.Album).WithMany(p => p.Vinyls)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vinyl_Album");

            entity.HasOne(d => d.Genre).WithMany(p => p.Vinyls)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vinyl_Genre");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
