using System.Security.Claims;
using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IReservationsService reservationsService) : ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<ActionResult> CreateReservation(ReservationRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var reservation = await reservationsService.CreateReservation(request, userId);

            return Ok(new
            {
                success = true,
                message = "Reservation created successfully",
                reservation = new
                {
                    id = reservation.Id,
                    date = reservation.ReservationDate,
                    status = reservation.Status
                }
            });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }
}