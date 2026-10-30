using System;
using System.Collections.Generic;
using System.Text;


using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class CrearArtistDtoValidator : AbstractValidator<ArtistDto>
    {
        public CrearArtistDtoValidator()
        {
            Include(new ArtistDtoValidator());

            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage("El ID debe ser 0 al registrar un artista.");
        }
    }
}
