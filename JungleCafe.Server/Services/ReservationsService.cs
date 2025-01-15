using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class ReservationsService(CafeDbContext context) : IReservationsService
{
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

    public async Task<Reservation> CreateReservation(ReservationRequest request, int userId)
    {
        var isAvailable = await IsTableAvailable(request.TableId, request.ReservationDate, request.Duration);
        if (!isAvailable) throw new InvalidOperationException("Table is already reserved for this time");

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
}