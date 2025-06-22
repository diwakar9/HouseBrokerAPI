using FluentValidation;
using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Validators;

public class CreatePropertyValidator : AbstractValidator<CreatePropertyDto>
{
    public CreatePropertyValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(300).WithMessage("Address cannot exceed 300 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required")
            .MaximumLength(100).WithMessage("State cannot exceed 100 characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Zip code is required")
            .MaximumLength(20).WithMessage("Zip code cannot exceed 20 characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters");

        RuleFor(x => x.Bedrooms)
            .GreaterThanOrEqualTo(0).WithMessage("Bedrooms must be 0 or greater");

        RuleFor(x => x.Bathrooms)
            .GreaterThanOrEqualTo(0).WithMessage("Bathrooms must be 0 or greater");

        RuleFor(x => x.SquareFeet)
            .GreaterThan(0).WithMessage("Square feet must be greater than 0");

        RuleFor(x => x.YearBuilt)
            .GreaterThan(1800).WithMessage("Year built must be greater than 1800")
            .LessThanOrEqualTo(DateTime.Now.Year + 5).WithMessage("Year built cannot be more than 5 years in the future");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");

        RuleFor(x => x.ImageUrls)
            .NotEmpty().WithMessage("At least one image is required");

        RuleForEach(x => x.ImageUrls)
            .Must(BeValidUrl).WithMessage("Image URL must be a valid URL");
    }

    private bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}

public class UpdatePropertyValidator : AbstractValidator<UpdatePropertyDto>
{
    public UpdatePropertyValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Property ID is required");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0");

        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .MaximumLength(300).WithMessage("Address cannot exceed 300 characters");

        RuleFor(x => x.City)
            .NotEmpty().WithMessage("City is required")
            .MaximumLength(100).WithMessage("City cannot exceed 100 characters");

        RuleFor(x => x.State)
            .NotEmpty().WithMessage("State is required")
            .MaximumLength(100).WithMessage("State cannot exceed 100 characters");

        RuleFor(x => x.ZipCode)
            .NotEmpty().WithMessage("Zip code is required")
            .MaximumLength(20).WithMessage("Zip code cannot exceed 20 characters");

        RuleFor(x => x.Country)
            .NotEmpty().WithMessage("Country is required")
            .MaximumLength(100).WithMessage("Country cannot exceed 100 characters");

        RuleFor(x => x.Bedrooms)
            .GreaterThanOrEqualTo(0).WithMessage("Bedrooms must be 0 or greater");

        RuleFor(x => x.Bathrooms)
            .GreaterThanOrEqualTo(0).WithMessage("Bathrooms must be 0 or greater");

        RuleFor(x => x.SquareFeet)
            .GreaterThan(0).WithMessage("Square feet must be greater than 0");

        RuleFor(x => x.YearBuilt)
            .GreaterThan(1800).WithMessage("Year built must be greater than 1800")
            .LessThanOrEqualTo(DateTime.Now.Year + 5).WithMessage("Year built cannot be more than 5 years in the future");

        RuleFor(x => x.Latitude)
            .InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90");

        RuleFor(x => x.Longitude)
            .InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180");

        RuleFor(x => x.ImageUrls)
            .NotEmpty().WithMessage("At least one image is required");

        RuleForEach(x => x.ImageUrls)
            .Must(BeValidUrl).WithMessage("Image URL must be a valid URL");
    }

    private bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}