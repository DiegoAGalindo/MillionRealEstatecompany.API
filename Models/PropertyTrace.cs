using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MillionRealEstatecompany.API.Models;

public class PropertyTrace
{
    [Key]
    public int IdPropertyTrace { get; set; }
    
    public DateTime DateSale { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Value { get; set; }
    
    [Column(TypeName = "decimal(18,2)")]
    public decimal Tax { get; set; }
    
    // Foreign Key
    [ForeignKey("Property")]
    public int IdProperty { get; set; }
    
    // Navigation property
    public virtual Property Property { get; set; } = null!;
}