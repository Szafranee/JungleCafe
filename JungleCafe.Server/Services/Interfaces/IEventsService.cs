using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;

namespace JungleCafe.Server.Services.Interfaces;

public interface IEventService
{
    Task<Event?> GetEvent(int id);
    Task<IEnumerable<Event>> GetEvents();
    Task RegisterForEvent(EventRegistrationRequest request, int userId);
}