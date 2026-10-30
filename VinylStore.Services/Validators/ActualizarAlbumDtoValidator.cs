using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class ActualizarAlbumDtoValidator : AbstractValidator<AlbumDto>
    {
        public ActualizarAlbumDtoValidator()
        {
            Include(new AlbumDtoValidator());

            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("El ID debe ser mayor que cero para actualizar un álbum.");
        }
    }
}
