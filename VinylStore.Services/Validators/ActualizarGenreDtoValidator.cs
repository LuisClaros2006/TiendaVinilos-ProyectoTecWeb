using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class ActualizarGenreDtoValidator : AbstractValidator<GenreDto>
    {
        public ActualizarGenreDtoValidator()
        {
            Include(new GenreDtoValidator());

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El ID debe ser mayor que cero para actualizar un género.");
        }
    }
}
