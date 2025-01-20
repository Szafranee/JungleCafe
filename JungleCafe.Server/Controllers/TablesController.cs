using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TablesController(ITablesService tablesService) : ControllerBase
{
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<TableAvailabilityDto>>> GetAvailableTables([FromQuery] DateTime dateTime)
    {
        try
        {
            var allTables = await tablesService.GetAllTables();
            var reservedTableIds = await tablesService.GetReservedTableIds(dateTime);

            var availableTables = allTables.Select(table => new TableAvailabilityDto
            {
                Id = table.Id,
                Number = table.Number,
                Capacity = table.Capacity,
                Zone = table.Zone,
                IsAvailable = !reservedTableIds.Contains(table.Id)
            });

            return Ok(availableTables);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
}