namespace HouseBroker.Domain.Entities;

public class Property
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public PropertyType PropertyType { get; set; }
    public decimal Price { get; set; }
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string ZipCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int Bedrooms { get; set; }
    public int Bathrooms { get; set; }
    public int SquareFeet { get; set; }
    public int YearBuilt { get; set; }
    public bool IsAvailable { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Foreign Keys
    public Guid BrokerId { get; set; }
    
    // Navigation Properties
    public virtual User Broker { get; set; } = null!;
    public virtual ICollection<PropertyImage> Images { get; set; } = new List<PropertyImage>();
    public virtual ICollection<PropertyFeature> Features { get; set; } = new List<PropertyFeature>();
}