using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> entity)
        {
            // Clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Nombre de la tabla en MySQL
            entity.ToTable("genre");

            // Índice único para el nombre del género
            entity.HasIndex(e => e.Name, "UQ_Genre_Name")
                .IsUnique();

            // Configuración de columnas
            entity.Property(e => e.Id)
                .HasColumnName("id");

            entity.Property(e => e.Name)
                .HasMaxLength(100);
        }
    }
}
