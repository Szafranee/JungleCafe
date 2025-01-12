using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JungleCafe.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController(IEventService eventService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<object>> GetEvent(int id)
        {
            var eventItem = await eventService.GetEvent(id);
            if (eventItem == null) return NotFound();
            return Ok(eventItem);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetEvents()
        {
            var events = await eventService.GetEvents();
            return Ok(events);
        }

        [HttpPost("register")]
        [Authorize]
        public async Task<ActionResult> RegisterForEvent([FromBody] EventRegistrationRequest request)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
                await eventService.RegisterForEvent(request, userId);
                return Ok(new { message = "Registration successful" });
            }
            catch (Exception)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}