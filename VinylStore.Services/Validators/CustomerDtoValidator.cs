using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class CustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public CustomerDtoValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("El nombre del cliente es obligatorio.")
                .MaximumLength(80)
                .WithMessage("El nombre no puede superar 80 caracteres.");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("El apellido del cliente es obligatorio.")
                .MaximumLength(80)
                .WithMessage("El apellido no puede superar 80 caracteres.");

            RuleFor(x => x.Email)
                .MaximumLength(150)
                .WithMessage("El correo no puede superar 150 caracteres.")
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email))
                .WithMessage("El correo electrónico no tiene un formato válido.");

            RuleFor(x => x.Phone)
                .MaximumLength(30)
                .WithMessage("El teléfono no puede superar 30 caracteres.");

            RuleFor(x => x.Address)
                .MaximumLength(250)
                .WithMessage("La dirección no puede superar 250 caracteres.");
        }
    }
}
