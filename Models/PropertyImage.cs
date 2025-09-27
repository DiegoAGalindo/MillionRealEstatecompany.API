using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionRealEstatecompany.API.Models;

public class PropertyImage
{
    [Key]
    public int IdPropertyImage { get; set; }
    
    // Foreign Key
    [ForeignKey("Property")]
    public int IdProperty { get; set; }
    
    [Required]
    [MaxLength(500)]
    public string File { get; set; } = string.Empty;
    
    public bool Enabled { get; set; } = true;
    
    // Navigation property
    public virtual Property Property { get; set; } = null!;
}