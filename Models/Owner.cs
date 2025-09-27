using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionRealEstatecompany.API.Models;

public class Owner
{
    [Key]
    public int IdOwner { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;
    
    [MaxLength(500)]
    public string? Photo { get; set; }
    
    public DateTime Birthday { get; set; }
    
    // Navigation property
    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}