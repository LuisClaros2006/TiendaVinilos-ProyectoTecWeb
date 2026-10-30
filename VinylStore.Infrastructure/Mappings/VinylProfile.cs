using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Mappings
{
    public class VinylProfile : Profile
    {
        public VinylProfile()
        {
            CreateMap<Vinyl, VinylDto>();

            CreateMap<VinylDto, Vinyl>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsActive,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Stock,
                    opt => opt.Ignore());
        }
    }
}