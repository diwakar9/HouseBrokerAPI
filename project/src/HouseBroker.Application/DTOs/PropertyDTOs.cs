using HouseBroker.Domain.Entities;

namespace HouseBroker.Application.DTOs;

public record CreatePropertyDto(
    string Title,
    string Description,
    PropertyType PropertyType,
    decimal Price,
    string Address,
    string City,
    string State,
    string ZipCode,
    string Country,
    double Latitude,
    double Longitude,
    int Bedrooms,
    int Bathrooms,
    int SquareFeet,
    int YearBuilt,
    List<string> ImageUrls,
    List<PropertyFeatureDto> Features
);

public record UpdatePropertyDto(
    Guid Id,
    string Title,
    string Description,
    PropertyType PropertyType,
    decimal Price,
    string Address,
    string City,
    string State,
    string ZipCode,
    string Country,
    double Latitude,
    double Longitude,
    int Bedrooms,
    int Bathrooms,
    int SquareFeet,
    int YearBuilt,
    bool IsAvailable,
    List<string> ImageUrls,
    List<PropertyFeatureDto> Features
);

public record PropertyDto(
    Guid Id,
    string Title,
    string Description,
    string PropertyType,
    decimal Price,
    string Address,
    string City,
    string State,
    string ZipCode,
    string Country,
    double Latitude,
    double Longitude,
    int Bedrooms,
    int Bathrooms,
    int SquareFeet,
    int YearBuilt,
    bool IsAvailable,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    UserDto Broker,
    List<PropertyImageDto> Images,
    List<PropertyFeatureDto> Features
);

public record PropertyImageDto(
    Guid Id,
    string ImageUrl,
    string Description,
    bool IsPrimary,
    int DisplayOrder
);

public record PropertyFeatureDto(
    string Name,
    string Description
);

public record PropertySearchDto(
    string? City,
    string? State,
    decimal? MinPrice,
    decimal? MaxPrice,
    string? PropertyType,
    int? MinBedrooms,
    int? MaxBedrooms,
    int? MinBathrooms,
    int? MaxBathrooms,
    int? MinSquareFeet,
    int? MaxSquareFeet,
    bool? IsAvailable,
    int PageNumber = 1,
    int PageSize = 10
);

public record PropertyListDto(
    Guid Id,
    string Title,
    string PropertyType,
    decimal Price,
    string City,
    string State,
    int Bedrooms,
    int Bathrooms,
    int SquareFeet,
    bool IsAvailable,
    string PrimaryImageUrl,
    UserDto Broker
);

public record PagedResultDto<T>(
    List<T> Items,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);