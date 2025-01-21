using System.Security.Claims;
using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(IEventService eventService) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        var events = await eventService.GetEvents();
        return Ok(events);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin, Manager, Employee, Caretaker")]
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        var eventItem = await eventService.GetEvent(id);
        if (eventItem == null)
        {
            return NotFound();
        }

        return Ok(eventItem);
    }

    [HttpPost("register")]
    [Authorize]
    public async Task<IActionResult> RegisterForEvent([FromBody] EventRegistrationRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            await eventService.RegisterForEvent(request, userId);
            return Ok(new { message = "Registration successful" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}