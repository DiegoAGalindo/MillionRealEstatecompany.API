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
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de la propiedad
    /// </summary>
    [Required(ErrorMessage = "La dirección de la propiedad es obligatoria")]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Precio de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El precio de la propiedad es obligatorio")]
    public decimal? Price { get; set; }

    /// <summary>
    /// Código interno de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El código interno es obligatorio")]
    public string CodeInternal { get; set; } = string.Empty;

    /// <summary>
    /// Año de construcción de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El año de construcción es obligatorio")]
    public int? Year { get; set; }

    /// <summary>
    /// ID del propietario de la propiedad
    /// </summary>
    [Required(ErrorMessage = "El ID del propietario es obligatorio")]
    public int? IdOwner { get; set; }
}

public class UpdatePropertyDto
{
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public int IdOwner { get; set; }
}

public class PropertyDetailDto : PropertyDto
{
    public List<PropertyImageDto> Images { get; set; } = new();
    public List<PropertyTraceDto> Traces { get; set; } = new();
}