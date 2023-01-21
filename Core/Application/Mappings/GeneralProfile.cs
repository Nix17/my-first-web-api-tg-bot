using Application.DTO.Common;
using Application.Helpers;
using Application.Interfaces.Services;
using AutoMapper;
using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Mappings;

public class GeneralProfile : Profile
{
    private readonly IMemoryCacheExtended _cache;
    public GeneralProfile(IMemoryCacheExtended cache)
    {
        _cache = cache;

        CreateMap<AuditableBaseEntity, AuditableDTO>()
            .ForMember(dto => dto.Created, opt => opt.MapFrom(m => m.Created.ToString("dd-MM-yyyy HH:mm")))
            .ForMember(dto => dto.LastModified, opt => opt.MapFrom(m => m.LastModified.ToString("dd-MM-yyyy HH:mm")))
            ;
    }
}