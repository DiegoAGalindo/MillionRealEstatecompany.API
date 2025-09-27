using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Data;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Owner mappings
        CreateMap<Owner, OwnerDto>();
        CreateMap<CreateOwnerDto, Owner>();
        CreateMap<UpdateOwnerDto, Owner>();

        // Property mappings
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name));
        CreateMap<Property, PropertyDetailDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PropertyImages))
            .ForMember(dest => dest.Traces, opt => opt.MapFrom(src => src.PropertyTraces));
        CreateMap<CreatePropertyDto, Property>();
        CreateMap<UpdatePropertyDto, Property>();

        // PropertyImage mappings
        CreateMap<PropertyImage, PropertyImageDto>();
        CreateMap<CreatePropertyImageDto, PropertyImage>();
        CreateMap<UpdatePropertyImageDto, PropertyImage>();

        // PropertyTrace mappings
        CreateMap<PropertyTrace, PropertyTraceDto>();
        CreateMap<CreatePropertyTraceDto, PropertyTrace>();
        CreateMap<UpdatePropertyTraceDto, PropertyTrace>();
    }
}