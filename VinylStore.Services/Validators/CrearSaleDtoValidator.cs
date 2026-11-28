using System;
using System.Collections.Generic;
using System.Text;


using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class CrearSaleDtoValidator : AbstractValidator<SaleDto>
    {
        public CrearSaleDtoValidator()
        {
            // Reutilizar las validaciones generales
            Include(new SaleDtoValidator());

            // El identificador lo genera MySQL
            RuleFor(x => x.Id)
                .Equal(0)
                .WithMessage(
                    "El ID debe ser 0 al registrar una venta.");

            // Los identificadores de los detalles
            // también los genera MySQL
            RuleForEach(x => x.Saledetails)
                .ChildRules(detail =>
                {
                    detail.RuleFor(x => x.Id)
                        .Equal(0)
                        .WithMessage(
                            "El ID del detalle debe ser 0.");

                    detail.RuleFor(x => x.SaleId)
                        .Equal(0)
                        .WithMessage(
                            "El SaleId debe ser 0 al registrar una venta.");
                });
        }
    }
}
