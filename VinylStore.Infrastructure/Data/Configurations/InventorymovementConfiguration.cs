using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data.Configurations
{
    public class InventorymovementConfiguration
        : IEntityTypeConfiguration<Inventorymovement>
    {
        public void Configure(
            EntityTypeBuilder<Inventorymovement> entity)
        {
            // Clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Nombre de la tabla en MySQL
            entity.ToTable("inventorymovement");

            // Índices
            entity.HasIndex(
                e => e.EmployeeId,
                "IX_InventoryMovement_EmployeeId");

            entity.HasIndex(
                e => e.MovementDate,
                "IX_InventoryMovement_MovementDate");

            entity.HasIndex(
                e => e.SaleId,
                "IX_InventoryMovement_SaleId");

            entity.HasIndex(
                e => e.VinylId,
                "IX_InventoryMovement_VinylId");

            // Identificador
            entity.Property(e => e.Id)
                .HasColumnName("id");

            // Fecha del movimiento
            entity.Property(e => e.MovementDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            // Tipo de movimiento
            entity.Property(e => e.MovementType)
                .HasColumnType(
                    "enum('INITIAL','RESTOCK','ADJUSTMENT','SALE','SALE_CANCELLATION')");

            // Motivo del movimiento
            entity.Property(e => e.Reason)
                .HasMaxLength(250);

            // Relación Inventorymovement -> Employee
            entity.HasOne(d => d.Employee)
                .WithMany(p => p.Inventorymovements)
                .HasForeignKey(d => d.EmployeeId)
                .HasConstraintName("FK_InventoryMovement_Employee");

            // Relación Inventorymovement -> Sale
            entity.HasOne(d => d.Sale)
                .WithMany(p => p.Inventorymovements)
                .HasForeignKey(d => d.SaleId)
                .HasConstraintName("FK_InventoryMovement_Sale");

            // Relación Inventorymovement -> Vinyl
            entity.HasOne(d => d.Vinyl)
                .WithMany(p => p.Inventorymovements)
                .HasForeignKey(d => d.VinylId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InventoryMovement_Vinyl");
        }
    }
}
