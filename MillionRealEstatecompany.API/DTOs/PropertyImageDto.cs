using System.ComponentModel.DataAnnotations;

namespace MillionRealEstatecompany.API.DTOs;

/// <summary>
/// DTO que representa una imagen de propiedad en el sistema
/// </summary>
public class PropertyImageDto
{
    public int IdPropertyImage { get; set; }
    public int IdProperty { get; set; }
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}

/// <summary>
/// DTO para crear una nueva imagen de propiedad
/// </summary>
public class CreatePropertyImageDto
{
    /// <summary>
    /// ID de la propiedad a la que pertenece la imagen
    /// </summary>
    [Required(ErrorMessage = "El ID de la propiedad es obligatorio")]
    public int? IdProperty { get; set; }

    /// <summary>
    /// Ruta o nombre del archivo de imagen
    /// </summary>
    [Required(ErrorMessage = "El archivo de imagen es obligatorio")]
    public string File { get; set; } = string.Empty;

    /// <summary>
    /// Indica si la imagen está habilitada
    /// </summary>
    [Required(ErrorMessage = "El estado habilitado es obligatorio")]
    public bool? Enabled { get; set; } = true;
}

public class UpdatePropertyImageDto
{
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}