using HouseBroker.Domain.Entities;

namespace HouseBroker.Domain.Interfaces;

public interface IPropertyRepository
{
    Task<Property?> GetByIdAsync(Guid id);
    Task<IEnumerable<Property>> GetAllAsync();
    Task<IEnumerable<Property>> GetByBrokerIdAsync(Guid brokerId);
    Task<IEnumerable<Property>> SearchAsync(PropertySearchCriteria criteria);
    Task<Property> AddAsync(Property property);
    Task UpdateAsync(Property property);
    Task DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}

public class PropertySearchCriteria
{
    public string? City { get; set; }
    public string? State { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public PropertyType? PropertyType { get; set; }
    public int? MinBedrooms { get; set; }
    public int? MaxBedrooms { get; set; }
    public int? MinBathrooms { get; set; }
    public int? MaxBathrooms { get; set; }
    public int? MinSquareFeet { get; set; }
    public int? MaxSquareFeet { get; set; }
    public bool? IsAvailable { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}