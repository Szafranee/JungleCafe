using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class EventsService(CafeDbContext context) : IEventService
{
    public async Task<IEnumerable<Event>> GetEvents()
    {
        return await context.Events.ToListAsync();
    }

    public async Task<Event?> GetEvent(int id)
    {
        return await context.Events.FindAsync(id);
    }

    public async Task RegisterForEvent(EventRegistrationRequest request, int userId)
    {
        var eventItem = await context.Events.FindAsync(request.EventId);
        if (eventItem == null)
            throw new Exception("Event not found");

        // TODO: Implement registration logic
        // This was just a placeholder in the original code too
    }
}