using System;
using System.Collections.Generic;
using System.Text;

using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class VinylDtoValidator : AbstractValidator<VinylDto>
    {
        public VinylDtoValidator()
        {
            // Validar álbum
            RuleFor(x => x.AlbumId)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un álbum válido.");

            // Validar género
            RuleFor(x => x.GenreId)
                .GreaterThan(0)
                .WithMessage("Debe seleccionar un género válido.");

            // Validar SKU
            RuleFor(x => x.Sku)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("El SKU del vinilo es obligatorio.")
                .MaximumLength(40)
                .WithMessage("El SKU no puede superar 40 caracteres.");

            // Validar formato
            RuleFor(x => x.Format)
                .Must(format =>
                    !string.IsNullOrWhiteSpace(format) &&
                    new[] { "LP", "EP", "SINGLE" }
                        .Contains(format.Trim().ToUpperInvariant()))
                .WithMessage("El formato debe ser LP, EP o SINGLE.");

            // Validar edición
            RuleFor(x => x.Edition)
                .MaximumLength(120)
                .WithMessage("La edición no puede superar 120 caracteres.");

            // Validar descripción
            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage("La descripción no puede superar 500 caracteres.");

            // Validar URL de portada
            RuleFor(x => x.CoverImageUrl)
                .MaximumLength(500)
                .WithMessage("La URL no puede superar 500 caracteres.")
                .Must(BeAValidUrl)
                .WithMessage("La URL debe comenzar con http:// o https://.");

            // Validar precio
            RuleFor(x => x.Price)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El precio no puede ser negativo.");

            // Validar stock
            RuleFor(x => x.Stock)
                .GreaterThanOrEqualTo(0)
                .WithMessage("El stock no puede ser negativo.");
        }

        // Comprobar URL
        private static bool BeAValidUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return true;

            return Uri.TryCreate(
                url,
                UriKind.Absolute,
                out var result)
                && (result.Scheme == Uri.UriSchemeHttp ||
                    result.Scheme == Uri.UriSchemeHttps);
        }
    }
}
