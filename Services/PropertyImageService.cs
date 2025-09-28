using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Service for managing property image business logic
/// </summary>
public class PropertyImageService : IPropertyImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropertyImageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyAsync(int propertyId)
    {
        var images = await _unitOfWork.PropertyImages.GetImagesByPropertyAsync(propertyId);
        return _mapper.Map<IEnumerable<PropertyImageDto>>(images);
    }

    public async Task<PropertyImageDto?> GetImageByIdAsync(int id)
    {
        var image = await _unitOfWork.PropertyImages.GetByIdAsync(id);
        return image == null ? null : _mapper.Map<PropertyImageDto>(image);
    }

    public async Task<PropertyImageDto> CreatePropertyImageAsync(CreatePropertyImageDto createImageDto)
    {
        // Validar que los campos requeridos no sean null
        if (!createImageDto.IdProperty.HasValue)
            throw new ArgumentException("El ID de la propiedad es obligatorio");
        if (!createImageDto.Enabled.HasValue)
            throw new ArgumentException("El estado habilitado es obligatorio");

        var propertyExists = await _unitOfWork.Properties.ExistsAsync(createImageDto.IdProperty.Value);
        if (!propertyExists)
            throw new ArgumentException("Property not found.");

        var image = _mapper.Map<PropertyImage>(createImageDto);
        await _unitOfWork.PropertyImages.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PropertyImageDto>(image);
    }

    public async Task<PropertyImageDto?> UpdatePropertyImageAsync(int id, UpdatePropertyImageDto updateImageDto)
    {
        var existingImage = await _unitOfWork.PropertyImages.GetByIdAsync(id);
        if (existingImage == null)
            return null;

        _mapper.Map(updateImageDto, existingImage);
        await _unitOfWork.PropertyImages.UpdateAsync(existingImage);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PropertyImageDto>(existingImage);
    }

    public async Task<bool> DeletePropertyImageAsync(int id)
    {
        var image = await _unitOfWork.PropertyImages.GetByIdAsync(id);
        if (image == null)
            return false;

        await _unitOfWork.PropertyImages.DeleteAsync(image);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }
}