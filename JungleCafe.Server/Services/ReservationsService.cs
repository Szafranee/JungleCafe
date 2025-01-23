using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace JungleCafe.Server.Services;

public class ReservationsService(CafeDbContext context) : IReservationsService
{
    public async Task<IEnumerable<ReservationDto>> GetReservations()
    {
        return await context.Reservations
            .Include(r => r.User)
            .Include(r => r.Table)
            .Select(r => new ReservationDto
            {
                Id = r.Id,
                UserId = r.UserId,
                FirstName = r.User.FirstName,
                LastName = r.User.LastName,
                Email = r.User.Email,
                PhoneNumber = r.User.PhoneNumber,
                TableId = r.TableId,
                TableNumber = r.Table.Number,
                ReservationDate = r.ReservationDate,
                Duration = r.Duration,
                Status = r.Status,
                GuestCount = r.GuestsCount,
                Notes = r.Notes,
                CreatedAt = r.CreatedAt,
                CancellationReason = r.CancellationReason
            })
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<ReservationDto>> GetUserReservations(int userId)
    {
        return await context.Reservations
            .Include(r => r.User)
            .Include(r => r.Table)
            .Where(r => r.UserId == userId)
            .Select(r => new ReservationDto
            {
                Id = r.Id,
                UserId = r.UserId,
                FirstName = r.User.FirstName,
                LastName = r.User.LastName,
                Email = r.User.Email,
                PhoneNumber = r.User.PhoneNumber,
                TableId = r.TableId,
                TableNumber = r.Table.Number,
                ReservationDate = r.ReservationDate,
                Duration = r.Duration,
                Status = r.Status,
                GuestCount = r.GuestsCount,
                Notes = r.Notes,
                CreatedAt = r.CreatedAt,
                CancellationReason = r.CancellationReason
            })
            .OrderByDescending(r => r.ReservationDate)
            .ToListAsync();
    }

    public async Task<Reservation> CreateReservation(ReservationRequest request, int userId)
    {
        var isAvailable = await IsTableAvailable(request.TableId, request.ReservationDate, request.Duration);
        if (!isAvailable)
        {
            throw new InvalidOperationException("Table is already reserved for this time");
        }

        if (request.ReservationDate < DateTime.UtcNow.AddMinutes(30))
        {
            throw new InvalidOperationException("Reservation has to made at least 30 minutes in advance");
        }

        var table = await context.Tables.FindAsync(request.TableId);
        if (table == null) throw new InvalidOperationException("Table not found");

        if (table.Capacity < request.GuestCount)
            throw new InvalidOperationException(
                $"Table capacity ({table.Capacity}) is less than requested guests ({request.GuestCount})");

        var reservation = new Reservation
        {
            UserId = userId,
            TableId = request.TableId,
            ReservationDate = request.ReservationDate,
            Duration = request.Duration,
            GuestsCount = request.GuestCount,
            Notes = request.SpecialRequests,
            Status = "confirmed",
            CreatedAt = DateTime.UtcNow
        };

        context.Reservations.Add(reservation);
        await context.SaveChangesAsync();

        return reservation;
    }

    public async Task<Reservation> CancelReservation(int reservationId, string reason)
{
    var reservation = await context.Reservations.FindAsync(reservationId);
    if (reservation == null)
    {
        throw new InvalidOperationException("Reservation not found");
    }

    reservation.Status = "cancelled";
    reservation.CancellationReason = reason;

    await context.SaveChangesAsync();

    return reservation;
}

    public async Task<Reservation?> UpdateReservation(int reservationId, ReservationUpdateRequest request)
    {
        var existingReservation = await context.Reservations.FindAsync(reservationId);

        if (existingReservation == null)
        {
            return null;
        }

        existingReservation.ReservationDate = request.ReservationDate;
        existingReservation.GuestsCount = request.GuestCount;
        existingReservation.Notes = request.Notes;

        await context.SaveChangesAsync();

        return existingReservation;
    }

    public async Task<Reservation> CompleteReservation(int reservationId)
    {
        var existingReservation = await context.Reservations.FindAsync(reservationId);
        if (existingReservation == null)
        {
            throw new InvalidOperationException("Reservation not found");
        }

        existingReservation.Status = "completed";
        await context.SaveChangesAsync();

        return existingReservation;
    }

    public async Task<bool> IsTableAvailable(int tableId, DateTime reservationDate, int duration)
    {
        var reservedTables = await context.Reservations
            .Where(r => r.TableId == tableId &&
                        r.ReservationDate >= reservationDate.AddMinutes(-90) &&
                        r.ReservationDate <= reservationDate.AddMinutes(90) &&
                        r.Status != "cancelled")
            .ToListAsync();

        return !reservedTables.Any();
    }

    public async Task<bool> CanUserModifyReservation(int userId, int reservationId)
    {
        var reservation = await context.Reservations
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null)
            return false;

        // Check if the reservation belongs to the user
        return reservation.UserId == userId;
    }
}