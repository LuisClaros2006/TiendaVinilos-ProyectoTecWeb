using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Mappings
{
    public class ArtistProfile : Profile
    {
        public ArtistProfile()
        {
            CreateMap<Artist, ArtistDto>();

            CreateMap<ArtistDto, Artist>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
        }
    }
}