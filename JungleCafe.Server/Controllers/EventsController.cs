using System.Security.Claims;
using JungleCafe.Server.DTOs;
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
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
    {
        var events = await eventService.GetEvents();
        return Ok(events);
    }

    [HttpGet("{id:int}")]
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

    [HttpPost]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<ActionResult<Event>> CreateEvent(EventCreateDto eventDto)
    {
        try
        {
            if (eventDto.EndDate <= eventDto.StartDate)
            {
                return BadRequest("End date must be later than start date");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

            var eventEntity = new Event
            {
                Title = eventDto.Title,
                Description = eventDto.Description,
                StartDate = eventDto.StartDate,
                EndDate = eventDto.EndDate,
                MaxParticipants = eventDto.MaxParticipants,
                ImageUrl = eventDto.ImageUrl,
                IsPublic = eventDto.IsPublic,
                CreatedBy = userId
            };

            var created = await eventService.CreateEvent(eventEntity);
            return CreatedAtAction(nameof(GetEvent), new { id = created.Id }, created);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<ActionResult<Event>> UpdateEvent(int id, [FromBody] EventUpdateDto eventDto)
    {
        Console.WriteLine(eventDto.ToString());

        var updated = await eventService.UpdateEvent(id, eventDto);
        if (updated == null)
        {
            return NotFound();
        }

        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        await eventService.DeleteEvent(id);
        return NoContent();
    }
}