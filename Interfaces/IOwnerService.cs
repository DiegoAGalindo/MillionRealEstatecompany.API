using MillionRealEstatecompany.API.DTOs;

namespace MillionRealEstatecompany.API.Interfaces;

/// <summary>
/// Interfaz para las operaciones del servicio de propietarios
/// </summary>
public interface IOwnerService
{
    Task<IEnumerable<OwnerDto>> GetAllOwnersAsync();
    Task<OwnerDto?> GetOwnerByIdAsync(int id);
    Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto);
    Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto updateOwnerDto);
    Task<bool> DeleteOwnerAsync(int id);
    Task<bool> OwnerExistsAsync(int id);
}