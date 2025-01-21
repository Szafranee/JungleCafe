using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;

namespace JungleCafe.Server.Services.Interfaces;

public interface IReservationsService
{
    Task<IEnumerable<ReservationDto>> GetReservations();
    Task<Reservation> CreateReservation(ReservationRequest request, int userId);
    Task<Reservation> CancelReservation(int reservationId, string reason);
    Task<Reservation?> UpdateReservation(int reservationId, ReservationUpdateRequest request);
    Task<Reservation> CompleteReservation(int reservationId);
    Task<bool> IsTableAvailable(int tableId, DateTime reservationDate, int duration);
}