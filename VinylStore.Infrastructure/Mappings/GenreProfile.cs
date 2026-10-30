using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Mappings
{
    public class GenreProfile : Profile
    {
        public GenreProfile()
        {
            CreateMap<Genre, GenreDto>();

            CreateMap<GenreDto, Genre>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore());
        }
    }
}