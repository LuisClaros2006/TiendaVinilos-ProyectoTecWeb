using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class AlbumDtoValidator : AbstractValidator<AlbumDto>
    {
        public AlbumDtoValidator()
        {
            RuleFor(x => x.Title)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("El título del álbum es obligatorio.")
                .MaximumLength(200)
                .WithMessage("El título del álbum no puede superar 200 caracteres.");

            RuleFor(x => x.ArtistId)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un artista válido.");

            RuleFor(x => x.ReleaseYear)
                .Must(year =>
                    !year.HasValue ||
                    (Convert.ToInt32(year.Value) >= 1900 &&
                     Convert.ToInt32(year.Value) <= 2100))
                .WithMessage("El año de lanzamiento debe estar entre 1900 y 2100.");
        }
    }
}
