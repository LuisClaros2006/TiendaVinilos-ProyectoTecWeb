using System;
using System.Collections.Generic;
using System.Text;


using AutoMapper;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Mappings
{
    public class SaleProfile : Profile
    {
        public SaleProfile()
        {
            // Convertir entidad Sale a SaleDto
            CreateMap<Sale, SaleDto>();

            // Convertir SaleDto a entidad Sale
            CreateMap<SaleDto, Sale>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.SaleDate,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Total,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Status,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CancelledAt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Customer,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Employee,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Inventorymovements,
                    opt => opt.Ignore());
        }
    }
}
