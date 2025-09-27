using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

public class PropertyService : IPropertyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropertyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
    {
        var properties = await _unitOfWork.Properties.GetPropertiesWithOwnerAsync();
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
    {
        var property = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(id);
        return property == null ? null : _mapper.Map<PropertyDto>(property);
    }

    public async Task<PropertyDetailDto?> GetPropertyWithDetailsAsync(int id)
    {
        var property = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(id);
        return property == null ? null : _mapper.Map<PropertyDetailDto>(property);
    }

    public async Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(int ownerId)
    {
        var properties = await _unitOfWork.Properties.GetPropertiesByOwnerAsync(ownerId);
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createPropertyDto)
    {
        // Validate owner exists
        var ownerExists = await _unitOfWork.Owners.ExistsAsync(createPropertyDto.IdOwner);
        if (!ownerExists)
            throw new ArgumentException("Owner not found.");

        // Validate unique CodeInternal
        var codeExists = await _unitOfWork.Properties.CodeInternalExistsAsync(createPropertyDto.CodeInternal);
        if (codeExists)
            throw new ArgumentException("CodeInternal already exists.");

        var property = _mapper.Map<Property>(createPropertyDto);
        await _unitOfWork.Properties.AddAsync(property);
        await _unitOfWork.SaveChangesAsync();

        // Return with owner information
        var createdProperty = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(property.IdProperty);
        return _mapper.Map<PropertyDto>(createdProperty);
    }

    public async Task<PropertyDto?> UpdatePropertyAsync(int id, UpdatePropertyDto updatePropertyDto)
    {
        var existingProperty = await _unitOfWork.Properties.GetByIdAsync(id);
        if (existingProperty == null)
            return null;

        // Validate owner exists
        var ownerExists = await _unitOfWork.Owners.ExistsAsync(updatePropertyDto.IdOwner);
        if (!ownerExists)
            throw new ArgumentException("Owner not found.");

        // Validate unique CodeInternal
        var codeExists = await _unitOfWork.Properties.CodeInternalExistsAsync(updatePropertyDto.CodeInternal, id);
        if (codeExists)
            throw new ArgumentException("CodeInternal already exists.");

        _mapper.Map(updatePropertyDto, existingProperty);
        await _unitOfWork.Properties.UpdateAsync(existingProperty);
        await _unitOfWork.SaveChangesAsync();

        // Return with owner information
        var updatedProperty = await _unitOfWork.Properties.GetPropertyWithDetailsAsync(id);
        return _mapper.Map<PropertyDto>(updatedProperty);
    }

    public async Task<bool> DeletePropertyAsync(int id)
    {
        var property = await _unitOfWork.Properties.GetByIdAsync(id);
        if (property == null)
            return false;

        await _unitOfWork.Properties.DeleteAsync(property);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> PropertyExistsAsync(int id)
    {
        return await _unitOfWork.Properties.ExistsAsync(id);
    }
}