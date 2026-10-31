using System;
using System.Collections.Generic;
using System.Text;

using AutoMapper;
using VinylStore.Core.DTOs;
using VinylStore.Core.Entities;

namespace VinylStore.Infrastructure.Mappings
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            CreateMap<Employee, EmployeeDto>();

            CreateMap<EmployeeDto, Employee>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsActive,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.Ignore());
        }
    }
}
