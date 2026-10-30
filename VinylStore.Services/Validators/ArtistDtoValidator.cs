using System;
using System.Collections.Generic;
using System.Text;


using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class ArtistDtoValidator : AbstractValidator<ArtistDto>
    {
        public ArtistDtoValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("El nombre del artista es obligatorio.")
                .MaximumLength(150)
                .WithMessage("El nombre del artista no puede superar 150 caracteres.");
        }
    }
}

