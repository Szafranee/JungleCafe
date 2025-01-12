using JungleCafe.Server.RequestModels;

namespace JungleCafe.Server.Services
{
    public interface IEventService
    {
        Task<object> GetEvent(int id);
        Task<IEnumerable<object>> GetEvents();
        Task RegisterForEvent(EventRegistrationRequest request, int userId);
    }
}