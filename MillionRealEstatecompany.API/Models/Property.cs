using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionRealEstatecompany.API.Models;

/// <summary>
/// Representa una propiedad inmobiliaria en el sistema
/// </summary>
public class Property
{
    /// <summary>
    /// Identificador único de la propiedad
    /// </summary>
    [Key]
    public int IdProperty { get; set; }

    /// <summary>
    /// Nombre o título de la propiedad
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección física de la propiedad
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// Precio de la propiedad en pesos colombianos
    /// </summary>
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }

    /// <summary>
    /// Código interno único de la propiedad
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string CodeInternal { get; set; } = string.Empty;

    /// <summary>
    /// Año de construcción de la propiedad
    /// </summary>
    public int Year { get; set; }

    /// <summary>
    /// Identificador del propietario de la propiedad
    /// </summary>
    [ForeignKey("Owner")]
    public int IdOwner { get; set; }

    /// <summary>
    /// Propietario de la propiedad
    /// </summary>
    public virtual Owner Owner { get; set; } = null!;

    /// <summary>
    /// Colección de imágenes asociadas a la propiedad
    /// </summary>
    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();

    /// <summary>
    /// Colección de trazas históricas de la propiedad
    /// </summary>
    public virtual ICollection<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
}