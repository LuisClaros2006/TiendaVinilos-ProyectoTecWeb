using System;
using System.Collections.Generic;
using System.Text;


using AutoMapper;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Mappings
{
    public class SaledetailProfile : Profile
    {
        public SaledetailProfile()
        {
            // Convertir entidad a DTO
            CreateMap<Saledetail, SaledetailDto>();

            // Convertir DTO a entidad
            CreateMap<SaledetailDto, Saledetail>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.SaleId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.UnitPrice,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Sale,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Vinyl,
                    opt => opt.Ignore());
        }
    }
}
