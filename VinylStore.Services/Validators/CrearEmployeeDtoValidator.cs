using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class CrearEmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public CrearEmployeeDtoValidator()
        {
            // Reutilizar las validaciones generales
            Include(new EmployeeDtoValidator());

            // El identificador lo genera MySQL
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage(
                    "El ID debe ser 0 al registrar un empleado.");
        }
    }
}

