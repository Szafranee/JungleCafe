using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;

namespace JungleCafe.Server.Services.Interfaces;

public interface IReservationsService
{
    Task<IEnumerable<ReservationDto>> GetReservations();
    Task<Reservation> CreateReservation(ReservationRequest request, int userId);
    Task<bool> IsTableAvailable(int tableId, DateTime reservationDate, int duration);
}