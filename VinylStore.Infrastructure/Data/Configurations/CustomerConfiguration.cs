using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data.Configurations
{
    public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> entity)
        {
            // Clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Nombre de la tabla en MySQL
            entity.ToTable("customer");

            // Identificador
            entity.Property(e => e.Id)
                .HasColumnName("id");

            // Dirección
            entity.Property(e => e.Address)
                .HasMaxLength(250);

            // Fecha de creación
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            // Correo electrónico
            entity.Property(e => e.Email)
                .HasMaxLength(150);

            // Nombre
            entity.Property(e => e.FirstName)
                .HasMaxLength(80);

            // Estado del cliente
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");

            // Apellido
            entity.Property(e => e.LastName)
                .HasMaxLength(80);

            // Teléfono
            entity.Property(e => e.Phone)
                .HasMaxLength(30);
        }
    }
}
