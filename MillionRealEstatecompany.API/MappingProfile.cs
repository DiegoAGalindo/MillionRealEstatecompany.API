using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API;

/// <summary>
/// Profile de AutoMapper para mapeo entre modelos y DTOs
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Property mappings
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Owner.IdOwner));
            
        CreateMap<Property, PropertyDetailDto>()
            .ForMember(dest => dest.OwnerName, opt => opt.MapFrom(src => src.Owner.Name))
            .ForMember(dest => dest.IdOwner, opt => opt.MapFrom(src => src.Owner.IdOwner))
            .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images));
            
        CreateMap<CreatePropertyDto, Property>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
            .ForMember(dest => dest.Owner, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            
        CreateMap<UpdatePropertyDto, Property>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // Owner mappings
        CreateMap<Owner, OwnerDto>();
        CreateMap<CreateOwnerDto, Owner>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdOwner, opt => opt.Ignore())
            .ForMember(dest => dest.PropertiesCount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
            
        CreateMap<UpdateOwnerDto, Owner>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdOwner, opt => opt.Ignore())
            .ForMember(dest => dest.PropertiesCount, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        // PropertyTrace mappings
        CreateMap<PropertyTrace, PropertyTraceDto>();
        CreateMap<CreatePropertyTraceDto, PropertyTrace>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdPropertyTrace, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());
            
        CreateMap<UpdatePropertyTraceDto, PropertyTrace>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.IdPropertyTrace, opt => opt.Ignore())
            .ForMember(dest => dest.PropertyId, opt => opt.Ignore())
            .ForMember(dest => dest.IdProperty, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        // PropertyImage mappings (embebido)
        CreateMap<EmbeddedPropertyImage, PropertyImageDto>();
        CreateMap<PropertyImageDto, EmbeddedPropertyImage>();
    }
}