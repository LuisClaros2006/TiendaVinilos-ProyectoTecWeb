using System;
using System.Collections.Generic;
using System.Text;


using FluentValidation;
using VinylStore.Core.DTOs;

namespace VinylStore.Services.Validators
{
    public class SaleDtoValidator : AbstractValidator<SaleDto>
    {
        public SaleDtoValidator()
        {
            // Validar cliente
            RuleFor(x => x.CustomerId)
                .GreaterThan(0)
                .WithMessage(
                    "Debe seleccionar un cliente válido.");

            // Validar empleado
            RuleFor(x => x.EmployeeId)
                .GreaterThan(0)
                .WithMessage(
                    "Debe seleccionar un empleado válido.");

            // La venta debe contener productos
            RuleFor(x => x.Saledetails)
                .NotNull()
                .WithMessage(
                    "La lista de productos es obligatoria.")
                .Must(details =>
                    details != null && details.Count > 0)
                .WithMessage(
                    "La venta debe incluir al menos un vinilo.");

            // Validar cada producto de la venta
            RuleForEach(x => x.Saledetails)
                .ChildRules(detail =>
                {
                    // Vinilo válido
                    detail.RuleFor(x => x.VinylId)
                        .GreaterThan(0)
                        .WithMessage(
                            "Debe seleccionar un vinilo válido.");

                    // Cantidad válida
                    detail.RuleFor(x => x.Quantity)
                        .GreaterThan(0)
                        .WithMessage(
                            "La cantidad debe ser mayor que cero.");
                });

            // No permitir vinilos repetidos
            RuleFor(x => x.Saledetails)
                .Must(details =>
                    details == null ||
                    details.Select(d => d.VinylId)
                        .Distinct()
                        .Count() == details.Count)
                .WithMessage(
                    "No puede repetir el mismo vinilo en una venta.");
        }
    }
}
