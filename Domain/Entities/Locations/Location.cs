namespace Career635.Domain.Entities.Locations;

// Level 1: Country
public class Country : BaseEntity {
    public string Name { get; set; } = string.Empty;
}

// Level 2: Province/State
public class Province : BaseEntity {
    public Guid? CountryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual Country? Country { get; set; } 
}

// Level 3: District
public class District : BaseEntity {
    public Guid? ProvinceId { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual Province? Province { get; set; }
}

// Level 4: Tehsil
public class City : BaseEntity {
    public Guid? DistrictId { get; set; }
    public string Name { get; set; } = string.Empty;
    public virtual District? District { get; set; }
}