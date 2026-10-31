using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
// using VinylStore.Infrastructure.Entities;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data;

using System.Reflection;
using Microsoft.EntityFrameworkCore;
using VinylStore.Core.Entities;

public partial class VinylStoreContext : DbContext
{
    public VinylStoreContext(
        DbContextOptions<VinylStoreContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<Artist> Artists { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<Inventorymovement> Inventorymovements { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<Saledetail> Saledetails { get; set; }

    public virtual DbSet<Vinyl> Vinyls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración global de MySQL
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        // Registrar las configuraciones automáticamente
        modelBuilder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly());

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
