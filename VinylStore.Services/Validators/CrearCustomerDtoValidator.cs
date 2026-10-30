using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class CrearCustomerDtoValidator : AbstractValidator<CustomerDto>
    {
        public CrearCustomerDtoValidator()
        {
            Include(new CustomerDtoValidator());

            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage(
                    "El ID debe ser 0 al registrar un cliente.");
        }
    }
}
