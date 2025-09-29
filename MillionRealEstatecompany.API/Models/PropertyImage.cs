using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionRealEstatecompany.API.Models;

/// <summary>
/// Representa una imagen asociada a una propiedad inmobiliaria
/// </summary>
public class PropertyImage
{
    /// <summary>
    /// Identificador único de la imagen
    /// </summary>
    [Key]
    public int IdPropertyImage { get; set; }

    /// <summary>
    /// Identificador de la propiedad asociada
    /// </summary>
    [ForeignKey("Property")]
    public int IdProperty { get; set; }

    /// <summary>
    /// URL o ruta del archivo de imagen
    /// </summary>
    [Required]
    [MaxLength(500)]
    public string File { get; set; } = string.Empty;

    /// <summary>
    /// Indica si la imagen está habilitada para mostrarse
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Propiedad asociada a esta imagen
    /// </summary>
    public virtual Property Property { get; set; } = null!;
}
