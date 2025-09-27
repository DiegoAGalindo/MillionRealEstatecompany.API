using MillionRealEstatecompany.API.DTOs;

namespace MillionRealEstatecompany.API.Interfaces;

public interface IPropertyImageService
{
    Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyAsync(int propertyId);
    Task<PropertyImageDto?> GetImageByIdAsync(int id);
    Task<PropertyImageDto> CreatePropertyImageAsync(CreatePropertyImageDto createImageDto);
    Task<PropertyImageDto?> UpdatePropertyImageAsync(int id, UpdatePropertyImageDto updateImageDto);
    Task<bool> DeletePropertyImageAsync(int id);
}