using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data.Configurations
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> entity)
        {
            // Clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Nombre de la tabla en MySQL
            entity.ToTable("album");

            // Índice de la clave foránea ArtistId
            entity.HasIndex(e => e.ArtistId, "IX_Album_ArtistId");

            // Identificador
            entity.Property(e => e.Id)
                .HasColumnName("id");

            // Título del álbum
            entity.Property(e => e.Title)
                .HasMaxLength(200);

            // Relación entre Album y Artist
            entity.HasOne(d => d.Artist)
                .WithMany(p => p.Albums)
                .HasForeignKey(d => d.ArtistId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Album_Artist");
        }
    }
}

