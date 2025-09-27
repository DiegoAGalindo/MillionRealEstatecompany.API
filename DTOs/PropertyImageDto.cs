namespace MillionRealEstatecompany.API.DTOs;

public class PropertyImageDto
{
    public int IdPropertyImage { get; set; }
    public int IdProperty { get; set; }
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}

public class CreatePropertyImageDto
{
    public int IdProperty { get; set; }
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; } = true;
}

public class UpdatePropertyImageDto
{
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}