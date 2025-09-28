using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Servicio para gestionar la lógica de negocio de los propietarios
/// </summary>
public class OwnerService : IOwnerService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public OwnerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync()
    {
        var owners = await _unitOfWork.Owners.GetAllAsync();
        return _mapper.Map<IEnumerable<OwnerDto>>(owners);
    }

    public async Task<OwnerDto?> GetOwnerByIdAsync(int id)
    {
        var owner = await _unitOfWork.Owners.GetByIdAsync(id);
        return owner == null ? null : _mapper.Map<OwnerDto>(owner);
    }

    public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto)
    {
        if (!createOwnerDto.Birthday.HasValue)
        {
            throw new ArgumentException("La fecha de nacimiento es obligatoria");
        }

        // Validar que el número de documento no exista (único campo que debe ser único)
        var documentExists = await _unitOfWork.Owners.DocumentNumberExistsAsync(createOwnerDto.DocumentNumber);
        if (documentExists)
        {
            throw new ArgumentException($"Ya existe un propietario con el número de documento '{createOwnerDto.DocumentNumber}'");
        }

        var owner = _mapper.Map<Owner>(createOwnerDto);
        await _unitOfWork.Owners.AddAsync(owner);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OwnerDto>(owner);
    }

    public async Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto updateOwnerDto)
    {
        var existingOwner = await _unitOfWork.Owners.GetByIdAsync(id);
        if (existingOwner == null)
            return null;

        // Validar que el número de documento no exista (debe ser único en toda la base de datos)
        // Si el documento ya existe y no es el mismo propietario, no permitir la actualización
        var documentExists = await _unitOfWork.Owners.DocumentNumberExistsAsync(updateOwnerDto.DocumentNumber);
        if (documentExists && existingOwner.DocumentNumber != updateOwnerDto.DocumentNumber)
        {
            throw new ArgumentException($"Ya existe un propietario con el número de documento '{updateOwnerDto.DocumentNumber}'");
        }

        _mapper.Map(updateOwnerDto, existingOwner);
        await _unitOfWork.Owners.UpdateAsync(existingOwner);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OwnerDto>(existingOwner);
    }

    /// <summary>
    /// Elimina un propietario del sistema validando que no tenga propiedades asociadas
    /// </summary>
    /// <param name="id">Identificador del propietario a eliminar</param>
    /// <returns>True si se eliminó exitosamente, False si no existe</returns>
    /// <exception cref="InvalidOperationException">Se lanza cuando el propietario tiene propiedades asociadas</exception>
    public async Task<bool> DeleteOwnerAsync(int id)
    {
        var owner = await _unitOfWork.Owners.GetByIdAsync(id);
        if (owner == null)
            return false;

        var hasProperties = await _unitOfWork.Owners.HasPropertiesAsync(id);
        if (hasProperties)
            throw new InvalidOperationException("Cannot delete owner with associated properties.");

        await _unitOfWork.Owners.DeleteAsync(owner);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    public async Task<bool> OwnerExistsAsync(int id)
    {
        return await _unitOfWork.Owners.ExistsAsync(id);
    }
}