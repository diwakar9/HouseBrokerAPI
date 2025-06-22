namespace HouseBroker.Domain.Entities;

public class PropertyImage
{
    public Guid Id { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public int DisplayOrder { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // Foreign Keys
    public Guid PropertyId { get; set; }
    
    // Navigation Properties
    public virtual Property Property { get; set; } = null!;
}