using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Services;

public interface IPropertyService
{
    Task<PropertyDto?> GetByIdAsync(Guid id);
    Task<PagedResultDto<PropertyListDto>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
    Task<PagedResultDto<PropertyListDto>> GetByBrokerIdAsync(Guid brokerId, int pageNumber = 1, int pageSize = 10);
    Task<PagedResultDto<PropertyListDto>> SearchAsync(PropertySearchDto searchDto);
    Task<PropertyDto> CreateAsync(CreatePropertyDto createDto, Guid brokerId);
    Task<PropertyDto> UpdateAsync(UpdatePropertyDto updateDto, Guid brokerId);
    Task DeleteAsync(Guid id, Guid brokerId);
}