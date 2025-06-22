using FluentValidation;
using HouseBroker.Application.DTOs;
using HouseBroker.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HouseBroker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly IValidator<CreatePropertyDto> _createValidator;
    private readonly IValidator<UpdatePropertyDto> _updateValidator;
    private readonly ILogger<PropertiesController> _logger;

    public PropertiesController(
        IPropertyService propertyService,
        IValidator<CreatePropertyDto> createValidator,
        IValidator<UpdatePropertyDto> updateValidator,
        ILogger<PropertiesController> logger)
    {
        _propertyService = propertyService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
        _logger = logger;
    }

    /// <summary>
    /// Get all properties with pagination
    /// </summary>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <returns>Paginated list of properties</returns>
    [HttpGet]
    public async Task<ActionResult<PagedResultDto<PropertyListDto>>> GetAllProperties(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var result = await _propertyService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting all properties");
            return StatusCode(500, new { Message = "An error occurred while retrieving properties" });
        }
    }

    /// <summary>
    /// Get property by ID
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <returns>Property details</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<PropertyDto>> GetProperty(Guid id)
    {
        try
        {
            var property = await _propertyService.GetByIdAsync(id);
            if (property == null)
            {
                return NotFound(new { Message = "Property not found" });
            }

            return Ok(property);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting property {PropertyId}", id);
            return StatusCode(500, new { Message = "An error occurred while retrieving the property" });
        }
    }

    /// <summary>
    /// Search properties with filters
    /// </summary>
    /// <param name="searchDto">Search criteria</param>
    /// <returns>Paginated filtered properties</returns>
    [HttpPost("search")]
    public async Task<ActionResult<PagedResultDto<PropertyListDto>>> SearchProperties([FromBody] PropertySearchDto searchDto)
    {
        try
        {
            var result = await _propertyService.SearchAsync(searchDto);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching properties");
            return StatusCode(500, new { Message = "An error occurred while searching properties" });
        }
    }

    /// <summary>
    /// Get properties by broker ID
    /// </summary>
    /// <param name="brokerId">Broker ID</param>
    /// <param name="pageNumber">Page number (default: 1)</param>
    /// <param name="pageSize">Page size (default: 10)</param>
    /// <returns>Paginated list of broker's properties</returns>
    [HttpGet("broker/{brokerId}")]
    public async Task<ActionResult<PagedResultDto<PropertyListDto>>> GetPropertiesByBroker(
        Guid brokerId,
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        try
        {
            if (pageNumber < 1) pageNumber = 1;
            if (pageSize < 1 || pageSize > 100) pageSize = 10;

            var result = await _propertyService.GetByBrokerIdAsync(brokerId, pageNumber, pageSize);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting properties for broker {BrokerId}", brokerId);
            return StatusCode(500, new { Message = "An error occurred while retrieving broker properties" });
        }
    }

    /// <summary>
    /// Create a new property (Broker only)
    /// </summary>
    /// <param name="createDto">Property creation data</param>
    /// <returns>Created property</returns>
    [HttpPost]
    [Authorize(Roles = "Broker")]
    public async Task<ActionResult<PropertyDto>> CreateProperty([FromBody] CreatePropertyDto createDto)
    {
        try
        {
            var validationResult = await _createValidator.ValidateAsync(createDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var brokerId))
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            var result = await _propertyService.CreateAsync(createDto, brokerId);
            _logger.LogInformation("Property created successfully by broker {BrokerId}: {PropertyId}", brokerId, result.Id);

            return CreatedAtAction(nameof(GetProperty), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating property");
            return StatusCode(500, new { Message = "An error occurred while creating the property" });
        }
    }

    /// <summary>
    /// Update property (Broker only - own properties)
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <param name="updateDto">Property update data</param>
    /// <returns>Updated property</returns>
    [HttpPut("{id}")]
    [Authorize(Roles = "Broker")]
    public async Task<ActionResult<PropertyDto>> UpdateProperty(Guid id, [FromBody] UpdatePropertyDto updateDto)
    {
        try
        {
            if (id != updateDto.Id)
            {
                return BadRequest(new { Message = "Property ID mismatch" });
            }

            var validationResult = await _updateValidator.ValidateAsync(updateDto);
            if (!validationResult.IsValid)
            {
                return BadRequest(new { Errors = validationResult.Errors.Select(e => e.ErrorMessage) });
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var brokerId))
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            var result = await _propertyService.UpdateAsync(updateDto, brokerId);
            _logger.LogInformation("Property updated successfully: {PropertyId}", id);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = "Property not found" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating property {PropertyId}", id);
            return StatusCode(500, new { Message = "An error occurred while updating the property" });
        }
    }

    /// <summary>
    /// Delete property (Broker only - own properties)
    /// </summary>
    /// <param name="id">Property ID</param>
    /// <returns>Success response</returns>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Broker")]
    public async Task<ActionResult> DeleteProperty(Guid id)
    {
        try
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var brokerId))
            {
                return Unauthorized(new { Message = "Invalid token" });
            }

            await _propertyService.DeleteAsync(id, brokerId);
            _logger.LogInformation("Property deleted successfully: {PropertyId}", id);

            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { Message = "Property not found" });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Forbid(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting property {PropertyId}", id);
            return StatusCode(500, new { Message = "An error occurred while deleting the property" });
        }
    }
}