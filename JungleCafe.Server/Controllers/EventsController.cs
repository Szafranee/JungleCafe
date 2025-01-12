using System.Security.Claims;
using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController(CafeDbContext context) : ControllerBase
{
    // GET: api/events/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<object>> GetEvent(int id)
    {
        var eventItem = await context.Events
            .Include(e => e.CreatedByNavigation)
            .FirstOrDefaultAsync(e => e.Id == id && e.IsPublic);

        if (eventItem == null) return NotFound();

        return new
        {
            eventItem.Id,
            eventItem.Title,
            eventItem.Description,
            eventItem.StartDate,
            eventItem.EndDate,
            eventItem.MaxParticipants,
            eventItem.ImageUrl,
            eventItem.IsPublic,
            CreatedBy = new
            {
                eventItem.CreatedByNavigation.Id,
                eventItem.CreatedByNavigation.FirstName,
                eventItem.CreatedByNavigation.LastName
            }
        };
    }

    // GET: api/events
    [HttpGet]
    public async Task<ActionResult<IEnumerable<object>>> GetEvents()
    {
        var events = await context.Events
            .Where(e => e.IsPublic)
            .OrderBy(e => e.StartDate)
            .Include(e => e.CreatedByNavigation)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Description,
                e.StartDate,
                e.EndDate,
                e.MaxParticipants,
                e.ImageUrl,
                e.IsPublic,
                CreatedBy = new
                {
                    e.CreatedByNavigation.Id,
                    e.CreatedByNavigation.FirstName,
                    e.CreatedByNavigation.LastName
                }
            })
            .ToListAsync();

        return Ok(events);
    }

    // POST: api/events/register
    [HttpPost("register")]
    [Authorize]
    public async Task<ActionResult> RegisterForEvent([FromBody] EventRegistrationRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var eventItem = await context.Events.FindAsync(request.EventId);

            if (eventItem == null) return NotFound("Event not found");

            // TODO: (maybe)
            // Check if user is already registered for the event
            // Check if the event is full

            return Ok(new { message = "Registration successful" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
}