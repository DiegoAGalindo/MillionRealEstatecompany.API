using AutoMapper;
using MillionRealEstatecompany.API.DTOs;
using MillionRealEstatecompany.API.Interfaces;
using MillionRealEstatecompany.API.Models;

namespace MillionRealEstatecompany.API.Services;

/// <summary>
/// Servicio para gestionar la lógica de negocio de las imágenes de propiedades
/// </summary>
public class PropertyImageService : IPropertyImageService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa una nueva instancia del servicio de imágenes de propiedades
    /// </summary>
    /// <param name="unitOfWork">Unidad de trabajo para el acceso a datos</param>
    /// <param name="mapper">Mapeador de objetos</param>
    public PropertyImageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /// <summary>
    /// Obtiene todas las imágenes asociadas a una propiedad específica
    /// </summary>
    /// <param name="propertyId">Identificador de la propiedad</param>
    /// <returns>Lista de imágenes de la propiedad</returns>
    public async Task<IEnumerable<PropertyImageDto>> GetImagesByPropertyAsync(int propertyId)
    {
        var images = await _unitOfWork.PropertyImages.GetImagesByPropertyAsync(propertyId);
        return _mapper.Map<IEnumerable<PropertyImageDto>>(images);
    }

    /// <summary>
    /// Obtiene una imagen específica por su identificador
    /// </summary>
    /// <param name="id">Identificador de la imagen</param>
    /// <returns>Imagen encontrada o null si no existe</returns>
    public async Task<PropertyImageDto?> GetImageByIdAsync(int id)
    {
        var image = await _unitOfWork.PropertyImages.GetByIdAsync(id);
        return image == null ? null : _mapper.Map<PropertyImageDto>(image);
    }

    /// <summary>
    /// Crea una nueva imagen para una propiedad
    /// </summary>
    /// <param name="createImageDto">Datos de la imagen a crear</param>
    /// <returns>La imagen creada</returns>
    /// <exception cref="ArgumentException">Se lanza cuando los datos de entrada son inválidos</exception>
    public async Task<PropertyImageDto> CreatePropertyImageAsync(CreatePropertyImageDto createImageDto)
    {
        if (!createImageDto.IdProperty.HasValue)
            throw new ArgumentException("Property ID is required");
        if (!createImageDto.Enabled.HasValue)
            throw new ArgumentException("Enabled status is required");

        var propertyExists = await _unitOfWork.Properties.ExistsAsync(createImageDto.IdProperty.Value);
        if (!propertyExists)
            throw new ArgumentException("Property not found");

        var image = _mapper.Map<PropertyImage>(createImageDto);
        await _unitOfWork.PropertyImages.AddAsync(image);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<PropertyImageDto>(image);
    }

    /// <summary>
    /// Actualiza una imagen existente de una propiedad
    /// </summary>
    /// <param name="id">Identificador de la imagen a actualizar</param>
    /// <param name="updateImageDto">Nuevos datos de la imagen</param>
    /// <returns>La imagen actualizada o null si no se encuentra</returns>
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

    /// <summary>
    /// Elimina una imagen de una propiedad
    /// </summary>
    /// <param name="id">Identificador de la imagen a eliminar</param>
    /// <returns>True si se eliminó correctamente, false si no se encontró</returns>
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