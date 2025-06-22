namespace HouseBroker.Domain.Entities;

public class PropertyFeature
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    
    // Foreign Keys
    public Guid PropertyId { get; set; }
    
    // Navigation Properties
    public virtual Property Property { get; set; } = null!;
}