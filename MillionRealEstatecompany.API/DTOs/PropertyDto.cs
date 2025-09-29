using System.ComponentModel.DataAnnotations;

namespace MillionRealEstatecompany.API.DTOs;

/// <summary>
/// DTO que representa una propiedad en el sistema
/// </summary>
public class PropertyDto
{
    public int IdProperty { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public int IdOwner { get; set; }
    public string OwnerName { get; set; } = string.Empty;
}

/// <summary>
/// DTO para crear una nueva propiedad
/// </summary>
public class CreatePropertyDto
{
    /// <summary>
    /// Nombre de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El nombre de la propiedad es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de la propiedad
    /// </summary>
    [Required(ErrorMessage = "La dirección de la propiedad es obligatoria")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "La dirección debe tener entre 10 y 200 caracteres")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Precio de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El precio de la propiedad es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Código interno de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El código interno es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El código interno debe tener entre 3 y 50 caracteres")]
    public string CodeInternal { get; set; } = string.Empty;

    /// <summary>
    /// Año de construcción de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El año de construcción es obligatorio")]
    [Range(1900, 2050, ErrorMessage = "El año de construcción debe estar entre 1900 y 2050")]
    public int? Year { get; set; }

    /// <summary>
    /// ID del propietario de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El ID del propietario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del propietario debe ser mayor a 0")]
    public int? IdOwner { get; set; }
}

/// <summary>
/// DTO para actualizar una propiedad existente
/// </summary>
public class UpdatePropertyDto
{
    /// <summary>
    /// Nombre de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El nombre de la propiedad es obligatorio")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de la propiedad
    /// </summary>
    [Required(ErrorMessage = "La dirección de la propiedad es obligatoria")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "La dirección debe tener entre 10 y 200 caracteres")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Precio de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El precio de la propiedad es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Price { get; set; }

    /// <summary>
    /// Código interno de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El código interno es obligatorio")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El código interno debe tener entre 3 y 50 caracteres")]
    public string CodeInternal { get; set; } = string.Empty;

    /// <summary>
    /// Año de construcción de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El año de construcción es obligatorio")]
    [Range(1900, 2050, ErrorMessage = "El año de construcción debe estar entre 1900 y 2050")]
    public int Year { get; set; }

    /// <summary>
    /// ID del propietario de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El ID del propietario es obligatorio")]
    [Range(1, int.MaxValue, ErrorMessage = "El ID del propietario debe ser mayor a 0")]
    public int IdOwner { get; set; }
}

/// <summary>
/// DTO para mostrar una propiedad con todos sus detalles
/// </summary>
public class PropertyDetailDto : PropertyDto
{
    public List<PropertyImageDto> Images { get; set; } = new();
    public List<PropertyTraceDto> Traces { get; set; } = new();
}

/// <summary>
/// DTO para actualizar únicamente el precio de una propiedad
/// </summary>
public class UpdatePropertyPriceDto
{
    /// <summary>
    /// Nuevo precio de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El precio es obligatorio")]
    [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
    public decimal Price { get; set; }
}

/// <summary>
/// DTO para filtros de búsqueda de propiedades
/// </summary>
public class PropertySearchFilter
{
    /// <summary>
    /// Precio mínimo de búsqueda
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "El precio mínimo debe ser mayor o igual a 0")]
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// Precio máximo de búsqueda
    /// </summary>
    [Range(0, double.MaxValue, ErrorMessage = "El precio máximo debe ser mayor o igual a 0")]
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// Año mínimo de construcción
    /// </summary>
    [Range(1900, 2050, ErrorMessage = "El año mínimo debe estar entre 1900 y 2050")]
    public int? MinYear { get; set; }

    /// <summary>
    /// Año máximo de construcción
    /// </summary>
    [Range(1900, 2050, ErrorMessage = "El año máximo debe estar entre 1900 y 2050")]
    public int? MaxYear { get; set; }

    /// <summary>
    /// ID del propietario para filtrar
    /// </summary>
    [Range(1, int.MaxValue, ErrorMessage = "El ID del propietario debe ser mayor a 0")]
    public int? OwnerId { get; set; }

    /// <summary>
    /// Ciudad o parte de la dirección para filtrar
    /// </summary>
    [StringLength(200, ErrorMessage = "La ciudad no puede exceder 200 caracteres")]
    public string? City { get; set; }

    /// <summary>
    /// Nombre o parte del nombre de la propiedad
    /// </summary>
    [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
    public string? Name { get; set; }
}