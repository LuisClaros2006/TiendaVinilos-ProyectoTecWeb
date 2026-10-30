using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class ActualizarVinylDtoValidator : AbstractValidator<VinylDto>
    {
        public ActualizarVinylDtoValidator()
        {
            // Reutilizar las validaciones generales
            Include(new VinylDtoValidator());

            // El vinilo debe tener un ID existente
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage(
                    "El ID debe ser mayor que cero para actualizar un vinilo.");
        }
    }
}
