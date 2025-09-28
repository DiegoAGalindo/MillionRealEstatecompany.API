using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Data;

/// <summary>
/// Perfil de AutoMapper para mapear entre entidades y DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Owner, OwnerDto>();
        CreateMap<CreateOwnerDto, Owner>();
        CreateMap<UpdateOwnerDto, Owner>();

        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name));
        CreateMap<Property, PropertyDetailDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PropertyImages))
            .ForMember(dest => dest.Traces, opt => opt.MapFrom(src => src.PropertyTraces));
        CreateMap<CreatePropertyDto, Property>();
        CreateMap<UpdatePropertyDto, Property>();

        CreateMap<PropertyImage, PropertyImageDto>();
        CreateMap<CreatePropertyImageDto, PropertyImage>();
        CreateMap<UpdatePropertyImageDto, PropertyImage>();

        CreateMap<PropertyTrace, PropertyTraceDto>();
        CreateMap<CreatePropertyTraceDto, PropertyTrace>();
        CreateMap<UpdatePropertyTraceDto, PropertyTrace>();
    }
}