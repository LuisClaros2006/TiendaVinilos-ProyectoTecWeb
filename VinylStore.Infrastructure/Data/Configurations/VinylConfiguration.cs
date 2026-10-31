using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data.Configurations
{
    public class VinylConfiguration : IEntityTypeConfiguration<Vinyl>
    {
        public void Configure(EntityTypeBuilder<Vinyl> entity)
        {
            // Clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Nombre de la tabla MySQL
            entity.ToTable("vinyl");

            // Índices
            entity.HasIndex(e => e.AlbumId, "IX_Vinyl_AlbumId");

            entity.HasIndex(e => e.GenreId, "IX_Vinyl_GenreId");

            entity.HasIndex(e => e.Sku, "UQ_Vinyl_Sku")
                .IsUnique();

            // Identificador
            entity.Property(e => e.Id)
                .HasColumnName("id");

            // URL de portada
            entity.Property(e => e.CoverImageUrl)
                .HasMaxLength(500);

            // Fecha de creación
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            // Descripción
            entity.Property(e => e.Description)
                .HasMaxLength(500);

            // Edición
            entity.Property(e => e.Edition)
                .HasMaxLength(120);

            // Formato del vinilo
            entity.Property(e => e.Format)
                .HasDefaultValueSql("'LP'")
                .HasColumnType("enum('LP','EP','SINGLE')");

            // Estado del vinilo
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");

            // Precio
            entity.Property(e => e.Price)
                .HasPrecision(10, 2);

            // SKU
            entity.Property(e => e.Sku)
                .HasMaxLength(40);

            // Relación Vinyl -> Album
            entity.HasOne(d => d.Album)
                .WithMany(p => p.Vinyls)
                .HasForeignKey(d => d.AlbumId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vinyl_Album");

            // Relación Vinyl -> Genre
            entity.HasOne(d => d.Genre)
                .WithMany(p => p.Vinyls)
                .HasForeignKey(d => d.GenreId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Vinyl_Genre");
        }
    }
}
