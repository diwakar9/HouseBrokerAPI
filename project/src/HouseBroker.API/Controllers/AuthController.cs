using FluentValidation;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace HouseBroker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IValidator<RegisterUserDto> _registerValidator;
    private readonly ILogger<AuthController> _logger;

    public AuthController(
        IAuthService authService,
        IValidator<RegisterUserDto> registerValidator,
        ILogger<AuthController> logger)
    {
        _authService = authService;
        _registerValidator = registerValidator;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user (House Seeker or Broker)
    /// </summary>
    /// <param name="registerDto">User registration information</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterUserDto registerDto)
    {
        try
        {
            var validationResult = await _registerValidator.ValidateAsync(registerDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var result = await _authService.RegisterAsync(registerDto);
            _logger.LogInformation("User registered successfully: {Email}", registerDto.Email);
            
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning("Registration failed: {Message}", ex.Message);
            return BadRequest(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user registration");
            return StatusCode(500, new { Message = "An error occurred during registration" });
        }
    }

    /// <summary>
    /// Login user
    /// </summary>
    /// <param name="loginDto">Login credentials</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { Message = "Email and password are required" });
            }

            var result = await _authService.LoginAsync(loginDto);
            _logger.LogInformation("User logged in successfully: {Email}", loginDto.Email);
            
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning("Login failed: {Message}", ex.Message);
            return Unauthorized(new { Message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during user login");
            return StatusCode(500, new { Message = "An error occurred during login" });
        }
    }

    /// <summary>
    /// Get current user information
    /// </summary>
    /// <returns>Current user information</returns>
    [HttpGet("me")]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        try
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            var user = await _authService.GetUserByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { Message = "User not found" });
            }

            return Ok(user);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting current user");
            return StatusCode(500, new { Message = "An error occurred while retrieving user information" });
        }
    }
}