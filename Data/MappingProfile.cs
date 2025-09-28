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
        CreateMap<CreateOwnerDto, Owner>()
            .ForMember(dest => dest.Birthday, opt => opt.MapFrom(src => src.Birthday!.Value));
        CreateMap<UpdateOwnerDto, Owner>();

        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name));
        CreateMap<Property, PropertyDetailDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.PropertyImages))
            .ForMember(dest => dest.Traces, opt => opt.MapFrom(src => src.PropertyTraces));
        CreateMap<CreatePropertyDto, Property>()
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price!.Value))
            .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Year!.Value))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.IdOwner!.Value));
        CreateMap<UpdatePropertyDto, Property>();

        CreateMap<PropertyImage, PropertyImageDto>();
        CreateMap<CreatePropertyImageDto, PropertyImage>()
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty!.Value))
            .ForMember(dest => dest.Enabled, opt => opt.MapFrom(src => src.Enabled!.Value));
        CreateMap<UpdatePropertyImageDto, PropertyImage>();

        CreateMap<PropertyTrace, PropertyTraceDto>();
        CreateMap<CreatePropertyTraceDto, PropertyTrace>()
            .ForMember(dest => dest.DateSale, opt => opt.MapFrom(src => src.DateSale!.Value))
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Value!.Value))
            .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.Tax!.Value))
            .ForMember(dest => dest.IdProperty, opt => opt.MapFrom(src => src.IdProperty!.Value));
        CreateMap<UpdatePropertyTraceDto, PropertyTrace>();
    }
}