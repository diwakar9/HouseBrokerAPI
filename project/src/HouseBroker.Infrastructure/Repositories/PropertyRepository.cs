using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Interfaces;
using HouseBroker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HouseBroker.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly HouseBrokerDbContext _context;

    public PropertyRepository(HouseBrokerDbContext context)
    {
        _context = context;
    }

    public async Task<Property?> GetByIdAsync(Guid id)
    {
        return await _context.Properties
            .Include(p => p.Broker)
            .Include(p => p.Images)
            .Include(p => p.Features)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Property>> GetAllAsync()
    {
        return await _context.Properties
            .Include(p => p.Broker)
            .Include(p => p.Images)
            .Include(p => p.Features)
            .Where(p => p.IsAvailable)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> GetByBrokerIdAsync(Guid brokerId)
    {
        return await _context.Properties
            .Include(p => p.Broker)
            .Include(p => p.Images)
            .Include(p => p.Features)
            .Where(p => p.BrokerId == brokerId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<Property>> SearchAsync(PropertySearchCriteria criteria)
    {
        var query = _context.Properties
            .Include(p => p.Broker)
            .Include(p => p.Images)
            .Include(p => p.Features)
            .AsQueryable();

        if (!string.IsNullOrEmpty(criteria.City))
        {
            query = query.Where(p => p.City.ToLower().Contains(criteria.City.ToLower()));
        }

        if (!string.IsNullOrEmpty(criteria.State))
        {
            query = query.Where(p => p.State.ToLower().Contains(criteria.State.ToLower()));
        }

        if (criteria.MinPrice.HasValue)
        {
            query = query.Where(p => p.Price >= criteria.MinPrice.Value);
        }

        if (criteria.MaxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= criteria.MaxPrice.Value);
        }

        if (criteria.PropertyType.HasValue)
        {
            query = query.Where(p => p.PropertyType == criteria.PropertyType.Value);
        }

        if (criteria.MinBedrooms.HasValue)
        {
            query = query.Where(p => p.Bedrooms >= criteria.MinBedrooms.Value);
        }

        if (criteria.MaxBedrooms.HasValue)
        {
            query = query.Where(p => p.Bedrooms <= criteria.MaxBedrooms.Value);
        }

        if (criteria.MinBathrooms.HasValue)
        {
            query = query.Where(p => p.Bathrooms >= criteria.MinBathrooms.Value);
        }

        if (criteria.MaxBathrooms.HasValue)
        {
            query = query.Where(p => p.Bathrooms <= criteria.MaxBathrooms.Value);
        }

        if (criteria.MinSquareFeet.HasValue)
        {
            query = query.Where(p => p.SquareFeet >= criteria.MinSquareFeet.Value);
        }

        if (criteria.MaxSquareFeet.HasValue)
        {
            query = query.Where(p => p.SquareFeet <= criteria.MaxSquareFeet.Value);
        }

        if (criteria.IsAvailable.HasValue)
        {
            query = query.Where(p => p.IsAvailable == criteria.IsAvailable.Value);
        }

        return await query
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Property> AddAsync(Property property)
    {
        _context.Properties.Add(property);
        return property;
    }

    public async Task UpdateAsync(Property property)
    {
        _context.Properties.Update(property);
    }

    public async Task DeleteAsync(Guid id)
    {
        var property = await _context.Properties.FindAsync(id);
        if (property != null)
        {
            _context.Properties.Remove(property);
        }
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Properties.AnyAsync(p => p.Id == id);
    }
}