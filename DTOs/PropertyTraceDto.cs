namespace MillionRealEstatecompany.API.DTOs;

public class PropertyTraceDto
{
    public int IdPropertyTrace { get; set; }
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public int IdProperty { get; set; }
}

public class CreatePropertyTraceDto
{
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public int IdProperty { get; set; }
}

public class UpdatePropertyTraceDto
{
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public int IdProperty { get; set; }
}