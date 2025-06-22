using AutoMapper;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Interfaces;

namespace HouseBroker.Application.Services;

public class PropertyService : IPropertyService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PropertyService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<PropertyDto?> GetByIdAsync(Guid id)
    {
        var property = await _unitOfWork.Properties.GetByIdAsync(id);
        return property != null ? _mapper.Map<PropertyDto>(property) : null;
    }

    public async Task<PagedResultDto<PropertyListDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
    {
        var properties = await _unitOfWork.Properties.GetAllAsync();
        var propertyList = properties.ToList();
        
        var totalCount = propertyList.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
        var pagedProperties = propertyList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var propertyDtos = _mapper.Map<List<PropertyListDto>>(pagedProperties);

        return new PagedResultDto<PropertyListDto>(
            propertyDtos,
            totalCount,
            pageNumber,
            pageSize,
            totalPages
        );
    }

    public async Task<PagedResultDto<PropertyListDto>> GetByBrokerIdAsync(Guid brokerId, int pageNumber = 1, int pageSize = 10)
    {
        var properties = await _unitOfWork.Properties.GetByBrokerIdAsync(brokerId);
        var propertyList = properties.ToList();
        
        var totalCount = propertyList.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        
        var pagedProperties = propertyList
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var propertyDtos = _mapper.Map<List<PropertyListDto>>(pagedProperties);

        return new PagedResultDto<PropertyListDto>(
            propertyDtos,
            totalCount,
            pageNumber,
            pageSize,
            totalPages
        );
    }

    public async Task<PagedResultDto<PropertyListDto>> SearchAsync(PropertySearchDto searchDto)
    {
        var criteria = _mapper.Map<PropertySearchCriteria>(searchDto);
        var properties = await _unitOfWork.Properties.SearchAsync(criteria);
        var propertyList = properties.ToList();
        
        var totalCount = propertyList.Count;
        var totalPages = (int)Math.Ceiling(totalCount / (double)searchDto.PageSize);
        
        var pagedProperties = propertyList
            .Skip((searchDto.PageNumber - 1) * searchDto.PageSize)
            .Take(searchDto.PageSize)
            .ToList();

        var propertyDtos = _mapper.Map<List<PropertyListDto>>(pagedProperties);

        return new PagedResultDto<PropertyListDto>(
            propertyDtos,
            totalCount,
            searchDto.PageNumber,
            searchDto.PageSize,
            totalPages
        );
    }

    public async Task<PropertyDto> CreateAsync(CreatePropertyDto createDto, Guid brokerId)
    {
        var property = _mapper.Map<Property>(createDto);
        property.Id = Guid.NewGuid();
        property.BrokerId = brokerId;
        property.CreatedAt = DateTime.UtcNow;
        property.IsAvailable = true;

        // Add images
        for (int i = 0; i < createDto.ImageUrls.Count; i++)
        {
            property.Images.Add(new PropertyImage
            {
                Id = Guid.NewGuid(),
                PropertyId = property.Id,
                ImageUrl = createDto.ImageUrls[i],
                IsPrimary = i == 0,
                DisplayOrder = i + 1,
                CreatedAt = DateTime.UtcNow
            });
        }

        // Add features
        foreach (var featureDto in createDto.Features)
        {
            property.Features.Add(new PropertyFeature
            {
                Id = Guid.NewGuid(),
                PropertyId = property.Id,
                Name = featureDto.Name,
                Description = featureDto.Description,
                CreatedAt = DateTime.UtcNow
            });
        }

        var createdProperty = await _unitOfWork.Properties.AddAsync(property);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PropertyDto>(createdProperty);
    }

    public async Task<PropertyDto> UpdateAsync(UpdatePropertyDto updateDto, Guid brokerId)
    {
        var existingProperty = await _unitOfWork.Properties.GetByIdAsync(updateDto.Id);
        if (existingProperty == null)
        {
            throw new KeyNotFoundException("Property not found");
        }

        if (existingProperty.BrokerId != brokerId)
        {
            throw new UnauthorizedAccessException("You can only update your own properties");
        }

        _mapper.Map(updateDto, existingProperty);
        existingProperty.UpdatedAt = DateTime.UtcNow;

        await _unitOfWork.Properties.UpdateAsync(existingProperty);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<PropertyDto>(existingProperty);
    }

    public async Task DeleteAsync(Guid id, Guid brokerId)
    {
        var property = await _unitOfWork.Properties.GetByIdAsync(id);
        if (property == null)
        {
            throw new KeyNotFoundException("Property not found");
        }

        if (property.BrokerId != brokerId)
        {
            throw new UnauthorizedAccessException("You can only delete your own properties");
        }

        await _unitOfWork.Properties.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }
}