using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Servicio para gestionar la lógica de negocio de las propiedades con MongoDB
/// </summary>
public class PropertyService : IPropertyService
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public PropertyService(IPropertyRepository propertyRepository, IOwnerRepository ownerRepository, IMapper mapper)
    {
        _propertyRepository = propertyRepository ?? throw new ArgumentNullException(nameof(propertyRepository));
        _ownerRepository = ownerRepository ?? throw new ArgumentNullException(nameof(ownerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<PropertyDto>> GetAllPropertiesAsync()
    {
        var properties = await _propertyRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDto?> GetPropertyByIdAsync(int id)
    {
        var property = await _propertyRepository.GetByIdPropertyAsync(id);
        return property != null ? _mapper.Map<PropertyDto>(property) : null;
    }

    public async Task<PropertyDto> CreatePropertyAsync(CreatePropertyDto createDto)
    {
        // Verificar que el propietario existe
        var owner = await _ownerRepository.GetByIdOwnerAsync(createDto.IdOwner ?? 0);
        if (owner == null)
        {
            throw new ArgumentException($"Owner with ID {createDto.IdOwner} not found.");
        }

        var property = _mapper.Map<Property>(createDto);
        
        // Mapear owner embebido
        property.Owner = new EmbeddedOwner
        {
            IdOwner = owner.IdOwner,
            Name = owner.Name,
            Address = owner.Address,
            Photo = owner.Photo ?? string.Empty
        };

        var createdProperty = await _propertyRepository.CreateAsync(property);
        
        // Actualizar contador de propiedades del owner
        var ownerPropertiesCount = (await _propertyRepository.GetPropertiesByOwnerAsync(owner.IdOwner)).Count();
        await _ownerRepository.UpdatePropertiesCountAsync(owner.IdOwner, ownerPropertiesCount);
        
        return _mapper.Map<PropertyDto>(createdProperty);
    }

    public async Task<PropertyDto?> UpdatePropertyAsync(int id, UpdatePropertyDto updateDto)
    {
        var property = await _propertyRepository.GetByIdPropertyAsync(id);
        if (property == null)
        {
            return null;
        }

        // Si cambia el propietario, verificar que el nuevo existe
        if (updateDto.IdOwner != property.Owner.IdOwner)
        {
            var newOwner = await _ownerRepository.GetByIdOwnerAsync(updateDto.IdOwner);
            if (newOwner == null)
            {
                throw new ArgumentException($"Owner with ID {updateDto.IdOwner} not found.");
            }

            // Actualizar owner embebido
            property.Owner = new EmbeddedOwner
            {
                IdOwner = newOwner.IdOwner,
                Name = newOwner.Name,
                Address = newOwner.Address,
                Photo = newOwner.Photo ?? string.Empty
            };
        }

        _mapper.Map(updateDto, property);
        property.UpdatedAt = DateTime.UtcNow;

        var updatedProperty = await _propertyRepository.UpdateAsync(property.Id!, property);
        return updatedProperty != null ? _mapper.Map<PropertyDto>(updatedProperty) : null;
    }

    public async Task<bool> DeletePropertyAsync(int id)
    {
        var property = await _propertyRepository.GetByIdPropertyAsync(id);
        if (property == null)
        {
            return false;
        }

        var result = await _propertyRepository.DeleteAsync(property.Id!);
        
        if (result)
        {
            // Actualizar contador de propiedades del owner
            var ownerPropertiesCount = (await _propertyRepository.GetPropertiesByOwnerAsync(property.Owner.IdOwner)).Count();
            await _ownerRepository.UpdatePropertiesCountAsync(property.Owner.IdOwner, ownerPropertiesCount);
        }
        
        return result;
    }

    public async Task<IEnumerable<PropertyDto>> SearchPropertiesAsync(PropertySearchFilter filter)
    {
        var properties = await _propertyRepository.SearchPropertiesAsync(filter);
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<bool> PropertyExistsAsync(int id)
    {
        var property = await _propertyRepository.GetByIdPropertyAsync(id);
        return property != null;
    }

    public async Task<PropertyDetailDto?> GetPropertyWithDetailsAsync(int id)
    {
        var property = await _propertyRepository.GetByIdPropertyAsync(id);
        return property != null ? _mapper.Map<PropertyDetailDto>(property) : null;
    }

    public async Task<IEnumerable<PropertyDto>> GetPropertiesByOwnerAsync(int ownerId)
    {
        var properties = await _propertyRepository.GetPropertiesByOwnerAsync(ownerId);
        return _mapper.Map<IEnumerable<PropertyDto>>(properties);
    }

    public async Task<PropertyDto?> UpdatePropertyPriceAsync(int id, decimal newPrice)
    {
        var property = await _propertyRepository.GetByIdPropertyAsync(id);
        if (property == null) return null;

        property.Price = newPrice;
        property.UpdatedAt = DateTime.UtcNow;

        var updatedProperty = await _propertyRepository.UpdateAsync(property.Id!, property);
        return updatedProperty != null ? _mapper.Map<PropertyDto>(updatedProperty) : null;
    }
}