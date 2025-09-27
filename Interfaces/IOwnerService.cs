using MillionRealEstatecompany.API.DTOs;

namespace MillionRealEstatecompany.API.Interfaces;

public interface IOwnerService
{
    Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();
    Task<OwnerDto?> GetOwnerByIdAsync(int id);
    Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto);
    Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto updateOwnerDto);
    Task<bool> DeleteOwnerAsync(int id);
    Task<bool> OwnerExistsAsync(int id);
}