using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class CrearAlbumDtoValidator : AbstractValidator<AlbumDto>
    {
        public CrearAlbumDtoValidator()
        {
            Include(new AlbumDtoValidator());

            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage("El ID debe ser 0 al registrar un álbum.");
        }
    }
}

