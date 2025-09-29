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

    /// <summary>
    /// Constructor del servicio de propietarios
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para acceso a datos</param>
    /// <param name="mapper">Mapeador de objetos</param>
    public OwnerService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todos los propietarios del sistema
    /// </summary>
    /// <returns>Lista de propietarios</returns>
    public async Task<IEnumerable<OwnerDto>> GetAllOwnersAsync()
    {
        var owners = await _unitOfWork.Owners.GetAllAsync();
        return _mapper.Map<IEnumerable<OwnerDto>>(owners);
    }

    /// <summary>
    /// Obtiene un propietario por su identificador
    /// </summary>
    /// <param name="id">Identificador del propietario</param>
    /// <returns>Propietario encontrado o null si no existe</returns>
    public async Task<OwnerDto?> GetOwnerByIdAsync(int id)
    {
        var owner = await _unitOfWork.Owners.GetByIdAsync(id);
        return owner == null ? null : _mapper.Map<OwnerDto>(owner);
    }

    /// <summary>
    /// Obtiene un propietario específico por su número de documento de identificación
    /// </summary>
    /// <param name="documentNumber">Número de documento de identificación del propietario</param>
    /// <returns>Propietario encontrado o null si no existe</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el número de documento es nulo o vacío</exception>
    public async Task<OwnerDto?> GetOwnerByDocumentNumberAsync(string documentNumber)
    {
        if (string.IsNullOrWhiteSpace(documentNumber))
            throw new ArgumentException("Document number is required");

        var owner = await _unitOfWork.Owners.GetByDocumentNumberAsync(documentNumber);
        return owner == null ? null : _mapper.Map<OwnerDto>(owner);
    }

    /// <summary>
    /// Crea un nuevo propietario en el sistema
    /// </summary>
    /// <param name="createOwnerDto">Datos del propietario a crear</param>
    /// <returns>Propietario creado</returns>
    /// <exception cref="ArgumentException">Se lanza cuando los datos son inválidos</exception>
    public async Task<OwnerDto> CreateOwnerAsync(CreateOwnerDto createOwnerDto)
    {
        if (!createOwnerDto.Birthday.HasValue)
        {
            throw new ArgumentException("Birthday is required");
        }

        var documentExists = await _unitOfWork.Owners.DocumentNumberExistsAsync(createOwnerDto.DocumentNumber);
        if (documentExists)
        {
            throw new ArgumentException($"Owner with document number '{createOwnerDto.DocumentNumber}' already exists");
        }

        var owner = _mapper.Map<Owner>(createOwnerDto);
        await _unitOfWork.Owners.AddAsync(owner);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<OwnerDto>(owner);
    }

    /// <summary>
    /// Actualiza un propietario existente
    /// </summary>
    /// <param name="id">Identificador del propietario</param>
    /// <param name="updateOwnerDto">Datos actualizados del propietario</param>
    /// <returns>Propietario actualizado o null si no existe</returns>
    /// <exception cref="ArgumentException">Se lanza cuando el número de documento ya existe</exception>
    public async Task<OwnerDto?> UpdateOwnerAsync(int id, UpdateOwnerDto updateOwnerDto)
    {
        var existingOwner = await _unitOfWork.Owners.GetByIdAsync(id);
        if (existingOwner == null)
            return null;

        var documentExists = await _unitOfWork.Owners.DocumentNumberExistsAsync(updateOwnerDto.DocumentNumber);
        if (documentExists && existingOwner.DocumentNumber != updateOwnerDto.DocumentNumber)
        {
            throw new ArgumentException($"Owner with document number '{updateOwnerDto.DocumentNumber}' already exists");
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
            throw new InvalidOperationException("Cannot delete owner with associated properties");

        await _unitOfWork.Owners.DeleteAsync(owner);
        await _unitOfWork.SaveChangesAsync();
        return true;
    }

    /// <summary>
    /// Verifica si existe un propietario con el identificador especificado
    /// </summary>
    /// <param name="id">Identificador del propietario</param>
    /// <returns>True si existe, False en caso contrario</returns>
    public async Task<bool> OwnerExistsAsync(int id)
    {
        return await _unitOfWork.Owners.ExistsAsync(id);
    }
}