using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;

namespace JungleCafe.Server.Services.Interfaces;

public interface IReservationsService
{
    Task<bool> IsTableAvailable(int tableId, DateTime reservationDate, int duration);
    Task<Reservation> CreateReservation(ReservationRequest request, int userId);
}