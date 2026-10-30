using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class CrearVinylDtoValidator : AbstractValidator<VinylDto>
    {
        public CrearVinylDtoValidator()
        {
            // Reutilizar las validaciones generales
            Include(new VinylDtoValidator());

            // El ID se genera en MySQL
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage(
                    "El ID debe ser 0 al registrar un vinilo.");

            // El inventario inicial será cero
            RuleFor(x => x.Stock)
                .Equal(0)
                .WithMessage(
                    "El stock inicial debe ser 0. Las existencias se gestionarán desde inventario.");
        }
    }
}
