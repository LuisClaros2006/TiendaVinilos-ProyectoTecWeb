using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> entity)
        {
            // Clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Nombre de la tabla en MySQL
            entity.ToTable("sale");

            // Índices
            entity.HasIndex(e => e.CustomerId, "IX_Sale_CustomerId");

            entity.HasIndex(e => e.EmployeeId, "IX_Sale_EmployeeId");

            entity.HasIndex(e => e.SaleDate, "IX_Sale_SaleDate");

            // Identificador
            entity.Property(e => e.Id)
                .HasColumnName("id");

            // Fecha de cancelación
            entity.Property(e => e.CancelledAt)
                .HasColumnType("datetime");

            // Fecha de venta
            entity.Property(e => e.SaleDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            // Estado de venta
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'COMPLETED'")
                .HasColumnType("enum('COMPLETED','CANCELLED')");

            // Importe total de la venta
            entity.Property(e => e.Total)
                .HasPrecision(12, 2);

            // Relación Sale -> Customer
            entity.HasOne(d => d.Customer)
                .WithMany(p => p.Sales)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Customer");

            // Relación Sale -> Employee
            entity.HasOne(d => d.Employee)
                .WithMany(p => p.Sales)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Sale_Employee");
        }
    }
}

