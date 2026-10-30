using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Mappings
{
    public class AlbumProfile : Profile
    {
        public AlbumProfile()
        {
            CreateMap<Album, AlbumDto>();

            CreateMap<AlbumDto, Album>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
        }
    }
}
