using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data.Configurations
{
    public class SaledetailConfiguration : IEntityTypeConfiguration<Saledetail>
    {
        public void Configure(EntityTypeBuilder<Saledetail> entity)
        {
            // Clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Nombre de la tabla en MySQL
            entity.ToTable("saledetail");

            // Índice de la clave foránea VinylId
            entity.HasIndex(e => e.VinylId, "IX_SaleDetail_VinylId");

            // Índice único compuesto
            entity.HasIndex(
                e => new { e.SaleId, e.VinylId },
                "UQ_SaleDetail_Sale_Vinyl")
                .IsUnique();

            // Identificador
            entity.Property(e => e.Id)
                .HasColumnName("id");

            // Precio unitario
            entity.Property(e => e.UnitPrice)
                .HasPrecision(10, 2);

            // Relación Saledetail -> Sale
            entity.HasOne(d => d.Sale)
                .WithMany(p => p.Saledetails)
                .HasForeignKey(d => d.SaleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetail_Sale");

            // Relación Saledetail -> Vinyl
            entity.HasOne(d => d.Vinyl)
                .WithMany(p => p.Saledetails)
                .HasForeignKey(d => d.VinylId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SaleDetail_Vinyl");
        }
    }
}
