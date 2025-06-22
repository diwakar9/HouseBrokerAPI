using FluentValidation;
using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Validators;

public class RegisterUserValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required")
            .EmailAddress().WithMessage("Email must be a valid email address");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long")
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]")
            .WithMessage("Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character");

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name is required")
            .MaximumLength(50).WithMessage("First name cannot exceed 50 characters");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name is required")
            .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters");

        RuleFor(x => x.Role)
            .NotEmpty().WithMessage("Role is required")
            .Must(BeValidRole).WithMessage("Role must be either 'HouseSeeker' or 'Broker'");
    }

    private bool BeValidRole(string role)
    {
        return role.Equals("HouseSeeker", StringComparison.OrdinalIgnoreCase) ||
               role.Equals("Broker", StringComparison.OrdinalIgnoreCase);
    }
}