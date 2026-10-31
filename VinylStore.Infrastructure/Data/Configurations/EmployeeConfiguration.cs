using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Data.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> entity)
        {
            // Clave primaria
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            // Nombre de la tabla en MySQL
            entity.ToTable("employee");

            // Índice único para el correo electrónico
            entity.HasIndex(e => e.Email, "UQ_Employee_Email")
                .IsUnique();

            // Identificador
            entity.Property(e => e.Id)
                .HasColumnName("id");

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

            // Estado del empleado
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");

            // Apellido
            entity.Property(e => e.LastName)
                .HasMaxLength(80);

            // Campo reservado para autenticación
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(255);

            // Rol del empleado
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'EMPLOYEE'")
                .HasColumnType("enum('ADMIN','EMPLOYEE')");
        }
    }
}

