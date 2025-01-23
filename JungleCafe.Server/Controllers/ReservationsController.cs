using System.Security.Claims;
using JungleCafe.Server.DTOs;
using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JungleCafe.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationsController(IReservationsService reservationsService) : ControllerBase
{

    [HttpGet]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetReservations()
    {
        try
        {
            var reservations = await reservationsService.GetReservations();
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

    [HttpGet("user")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<ReservationDto>>> GetUserReservations()
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var reservations = await reservationsService.GetUserReservations(userId);
            return Ok(reservations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal server error" });
        }
    }

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

    [HttpPost("{id:int}/cancel")]
    [Authorize]
    public async Task<ActionResult> CancelReservation(int id, [FromBody] ReservationCancellationRequest request)
    {
        try
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Check if user has permission to cancel this reservation
            if (!User.IsInRole("Admin") && !User.IsInRole("Manager") && !User.IsInRole("Employee"))
            {
                var canModify = await reservationsService.CanUserModifyReservation(userId, id);
                if (!canModify)
                {
                    return Forbid();
                }
            }

            await reservationsService.CancelReservation(id, request.Reason);
            return Ok(new { message = "Reservation cancelled successfully" });
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

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<ActionResult> UpdateReservation(int id, ReservationUpdateRequest request)
    {
        try
        {
            var reservation = await reservationsService.UpdateReservation(id, request);
            return Ok(new
            {
                success = true,
                message = "Reservation updated successfully",
                reservation
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

    [HttpPost("{id:int}/complete")]
    [Authorize(Roles = "Admin, Manager, Employee")]
    public async Task<ActionResult> CompleteReservation(int id)
    {
        try
        {
            await reservationsService.CompleteReservation(id);
            return Ok(new { message = "Reservation completed successfully" });
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