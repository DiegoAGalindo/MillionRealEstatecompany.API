using Microsoft.AspNetCore.Mvc;
using MillionRealEstatecompany.API.Interfaces;

namespace MillionRealEstatecompany.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DataSeederController : ControllerBase
{
    private readonly IDataSeeder _dataSeeder;
    private readonly ILogger<DataSeederController> _logger;

    public DataSeederController(IDataSeeder dataSeeder, ILogger<DataSeederController> logger)
    {
        _dataSeeder = dataSeeder;
        _logger = logger;
    }

    /// <summary>
    /// Check if the database has initial data
    /// </summary>
    [HttpGet("status")]
    public async Task<ActionResult<object>> GetDataStatus()
    {
        try
        {
            var hasData = await _dataSeeder.HasDataAsync();
            return Ok(new { hasData, message = hasData ? "Database contains data" : "Database is empty" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error checking data status");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Seed the database with initial data
    /// </summary>
    [HttpPost("seed")]
    public async Task<IActionResult> SeedData([FromQuery] bool force = false)
    {
        try
        {
            if (!force && await _dataSeeder.HasDataAsync())
            {
                return BadRequest(new { message = "Database already contains data. Use force=true to override." });
            }

            if (force)
            {
                await _dataSeeder.ClearDataAsync();
            }

            await _dataSeeder.SeedDataAsync();
            return Ok(new { message = "Database seeded successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error seeding data");
            return StatusCode(500, new { message = "Error seeding data", error = ex.Message });
        }
    }

    /// <summary>
    /// Clear all data from the database
    /// </summary>
    [HttpDelete("clear")]
    public async Task<IActionResult> ClearData([FromQuery] bool confirm = false)
    {
        if (!confirm)
        {
            return BadRequest(new { message = "Please confirm the operation by setting confirm=true" });
        }

        try
        {
            await _dataSeeder.ClearDataAsync();
            return Ok(new { message = "Database cleared successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing data");
            return StatusCode(500, new { message = "Error clearing data", error = ex.Message });
        }
    }
}