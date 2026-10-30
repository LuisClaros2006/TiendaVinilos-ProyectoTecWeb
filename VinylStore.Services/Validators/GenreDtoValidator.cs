using System;
using System.Collections.Generic;
using System.Text;


using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class GenreDtoValidator : AbstractValidator<GenreDto>
    {
        public GenreDtoValidator()
        {
            RuleFor(x => x.Name)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .WithMessage("El nombre del género es obligatorio.")
                .MaximumLength(100)
                .WithMessage("El nombre del género no puede superar 100 caracteres.");
        }
    }
}
