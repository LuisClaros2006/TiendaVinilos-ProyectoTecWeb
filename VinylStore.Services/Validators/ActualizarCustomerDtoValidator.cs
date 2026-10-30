using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class ActualizarCustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public ActualizarCustomerDtoValidator()
        {
            Include(new CustomerDtoValidator());

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    "El ID debe ser mayor que cero para actualizar.");

        }
    }
}