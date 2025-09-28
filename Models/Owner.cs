using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionRealEstatecompany.API.Models;

/// <summary>
/// Representa un propietario de inmuebles en el sistema
/// </summary>
public class Owner
{
    /// <summary>
    /// Identificador único del propietario
    /// </summary>
    [Key]
    public int IdOwner { get; set; }

    /// <summary>
    /// Nombre completo del propietario
    /// </summary>
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Dirección de residencia del propietario
    /// </summary>
    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;

    /// <summary>
    /// URL de la foto del propietario (opcional)
    /// </summary>
    [MaxLength(500)]
    public string? Photo { get; set; }

    /// <summary>
    /// Fecha de nacimiento del propietario (solo fecha, sin hora)
    /// </summary>
    public DateOnly Birthday { get; set; }

    /// <summary>
    /// Número de documento de identificación del propietario (único)
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string DocumentNumber { get; set; } = string.Empty;

    /// <summary>
    /// Correo electrónico del propietario
    /// </summary>
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// Colección de propiedades que posee este propietario
    /// </summary>
    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
