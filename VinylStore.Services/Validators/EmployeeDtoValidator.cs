using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
    {
        public EmployeeDtoValidator()
        {
            // Validar nombre
            RuleFor(x => x.FirstName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("El nombre del empleado es obligatorio.")
                .MaximumLength(80)
                .WithMessage("El nombre no puede superar 80 caracteres.");

            // Validar apellido
            RuleFor(x => x.LastName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("El apellido del empleado es obligatorio.")
                .MaximumLength(80)
                .WithMessage("El apellido no puede superar 80 caracteres.");

            // Validar correo electrónico
            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("El correo electrónico es obligatorio.")
                .MaximumLength(150)
                .WithMessage("El correo no puede superar 150 caracteres.")
                .EmailAddress()
                .WithMessage("El correo electrónico no tiene un formato válido.");

            // Validar rol
            RuleFor(x => x.Role)
                .Must(role =>
                    !string.IsNullOrWhiteSpace(role) &&
                    new[] { "ADMIN", "EMPLOYEE" }
                        .Contains(role.Trim().ToUpperInvariant()))
                .WithMessage("El rol debe ser ADMIN o EMPLOYEE.");
        }
    }
}

