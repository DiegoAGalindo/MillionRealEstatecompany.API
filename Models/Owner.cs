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
    /// Fecha de nacimiento del propietario
    /// </summary>
    public DateTime Birthday { get; set; }

    /// <summary>
    /// Colección de propiedades que posee este propietario
    /// </summary>
    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}
