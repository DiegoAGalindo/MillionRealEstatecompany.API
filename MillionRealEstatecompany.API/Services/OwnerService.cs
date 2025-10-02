using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

public class OwnerService : IOwnerService
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;

    public OwnerService(IOwnerRepository ownerRepository, IPropertyRepository propertyRepository, IMapper mapper)
    {
        _ownerRepository = ownerRepository ?? throw new ArgumentNullException(nameof(ownerRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync()
    {
        var owners = await _ownerRepository.GetAllAsync();
        return _mapper.Map<IEnumerable<OwnerDto>>(owners);
    }

    public async Task<OwnerDto?> GetOwnerByIdAsync(int id)
    {
        var owner = await _ownerRepository.GetByIdOwnerAsync(id);
        return owner != null ? _mapper.Map<OwnerDto>(owner) : null;
    }

    public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createDto)
    {
        var owner = _mapper.Map<Owner>(createDto);
        var createdOwner = await _ownerRepository.CreateAsync(owner);
        return _mapper.Map<OwnerDto>(createdOwner);
    }

    public async Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto updateDto)
    {
        var owner = await _ownerRepository.GetByIdOwnerAsync(id);
        if (owner == null) return null;

        _mapper.Map(updateDto, owner);
        owner.UpdatedAt = DateTime.UtcNow;

        var updatedOwner = await _ownerRepository.UpdateAsync(owner.Id!, owner);
        return updatedOwner != null ? _mapper.Map<OwnerDto>(updatedOwner) : null;
    }

    public async Task<bool> DeleteOwnerAsync(int id)
    {
        var owner = await _ownerRepository.GetByIdOwnerAsync(id);
        if (owner == null) return false;

        return await _ownerRepository.DeleteAsync(owner.Id!);
    }

    public async Task<bool> OwnerExistsAsync(int id)
    {
        var owner = await _ownerRepository.GetByIdOwnerAsync(id);
        return owner != null;
    }

    public async Task<OwnerDto?> GetOwnerByDocumentNumberAsync(string documentNumber)
    {
        var owner = await _ownerRepository.GetByDocumentNumberAsync(documentNumber);
        return owner != null ? _mapper.Map<OwnerDto>(owner) : null;
    }
}