namespace HouseBroker.Application.DTOs;

public record RegisterUserDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    string Role
);

public record LoginDto(
    string Email,
    string Password
);

public record UserDto(
    Guid Id,
    string Email,
    string FirstName,
    string LastName,
    string FullName,
    string Role,
    DateTime CreatedAt,
    bool IsActive
);

public record AuthResponseDto(
    string Token,
    UserDto User,
    DateTime ExpiresAt
);