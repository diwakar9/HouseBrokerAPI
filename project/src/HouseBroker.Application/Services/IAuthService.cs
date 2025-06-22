using HouseBroker.Application.DTOs;

namespace HouseBroker.Application.Services;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterUserDto registerDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<UserDto?> GetUserByIdAsync(Guid userId);
    Task<bool> ValidateTokenAsync(string token);
}