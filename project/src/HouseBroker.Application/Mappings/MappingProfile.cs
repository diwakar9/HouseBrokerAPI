using AutoMapper;
using HouseBroker.Application.DTOs;
using HouseBroker.Domain.Entities;
using HouseBroker.Domain.Interfaces;

namespace HouseBroker.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

        // Property mappings
        CreateMap<Property, PropertyDto>()
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.ToString()));

        CreateMap<Property, PropertyListDto>()
            .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => src.PropertyType.ToString()))
            .ForMember(dest => dest.PrimaryImageUrl, opt => opt.MapFrom(src =>
                src.Images.FirstOrDefault(i => i.IsPrimary) != null
                    ? src.Images.FirstOrDefault(i => i.IsPrimary)!.ImageUrl
                    : src.Images.FirstOrDefault() != null
                        ? src.Images.FirstOrDefault()!.ImageUrl
                        : string.Empty));

        CreateMap<CreatePropertyDto, Property>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.BrokerId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.IsAvailable, opt => opt.Ignore())
            .ForMember(dest => dest.Broker, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Features, opt => opt.Ignore());

        CreateMap<UpdatePropertyDto, Property>()
            .ForMember(dest => dest.BrokerId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Broker, opt => opt.Ignore())
            .ForMember(dest => dest.Images, opt => opt.Ignore())
            .ForMember(dest => dest.Features, opt => opt.Ignore());

        CreateMap<PropertyImage, PropertyImageDto>();
        CreateMap<PropertyFeature, PropertyFeatureDto>();

        // Search criteria mapping
        CreateMap<PropertySearchDto, PropertySearchCriteria>()
                    .ForMember(dest => dest.PropertyType, opt => opt.MapFrom(src => ParsePropertyType(src.PropertyType)));
    }

    private static PropertyType? ParsePropertyType(string propertyTypeString)
    {
        if (string.IsNullOrEmpty(propertyTypeString))
            return null;

        return Enum.TryParse<PropertyType>(propertyTypeString, true, out var propertyType)
            ? propertyType
            : (PropertyType?)null;
    }
}