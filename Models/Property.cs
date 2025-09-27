using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionRealEstatecompany.API.Models;

public class Property
{
    [Key]
    public int IdProperty { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(200)]
    public string Address { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string CodeInternal { get; set; } = string.Empty;
    
    public int Year { get; set; }
    
    // Foreign Key
    [ForeignKey("Owner")]
    public int IdOwner { get; set; }
    
    // Navigation properties
    public virtual Owner Owner { get; set; } = null!;
    public virtual ICollection<PropertyImage> PropertyImages { get; set; } = new List<PropertyImage>();
    public virtual ICollection<PropertyTrace> PropertyTraces { get; set; } = new List<PropertyTrace>();
}