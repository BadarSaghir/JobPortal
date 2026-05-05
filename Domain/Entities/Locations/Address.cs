namespace Career635.Domain.Entities.Locations;

public class Address : BaseEntity
{
    public Guid CountryId { get; set; }
    public Guid ProvinceId { get; set; }
    public Guid DistrictId { get; set; }
    public Guid TehsilId { get; set; }
    
    public string StreetAddress { get; set; } = string.Empty; // House #, Street #, Sector

    // Navigations
    public virtual Country Country { get; set; } = null!;
    public virtual Province Province { get; set; } = null!;
    public virtual District District { get; set; } = null!;
    public virtual City City { get; set; } = null!;
}