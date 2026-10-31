using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class ActualizarEmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public ActualizarEmployeeDtoValidator()
        {
            // Reutilizar las validaciones generales
            Include(new EmployeeDtoValidator());

            // El empleado debe tener un identificador
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    "El ID debe ser mayor que cero para actualizar un empleado.");
        }
    }
}
