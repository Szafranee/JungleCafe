using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;

namespace JungleCafe.Server.Services.Interfaces;

public interface IEventService
{
    Task<Event?> GetEvent(int id);
    Task<IEnumerable<EventDto>> GetEvents();
    Task RegisterForEvent(EventRegistrationRequest request, int userId);
    Task<EventDto> CreateEvent(Event eventItem);
    Task<Event?> UpdateEvent(int id, EventUpdateDto eventDto);
    Task DeleteEvent(int id);
}