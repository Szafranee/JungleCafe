using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class EventsService(CafeDbContext context) : IEventService
{
    public async Task<object> GetEvent(int id)
    {
        var eventItem = await context.Events
            .Include(e => e.CreatedByNavigation)
            .FirstOrDefaultAsync(e => e.Id == id && e.IsPublic);

        if (eventItem == null) return null;

        return new
        {
            eventItem.Id,
            eventItem.Title,
            eventItem.Description,
            eventItem.StartDate,
            eventItem.EndDate,
            eventItem.MaxParticipants,
            eventItem.ImageUrl,
            eventItem.IsPublic,
            CreatedBy = new
            {
                eventItem.CreatedByNavigation.Id,
                eventItem.CreatedByNavigation.FirstName,
                eventItem.CreatedByNavigation.LastName
            }
        };
    }

    public async Task<IEnumerable<object>> GetEvents()
    {
        return await context.Events
            .Where(e => e.IsPublic)
            .OrderBy(e => e.StartDate)
            .Include(e => e.CreatedByNavigation)
            .Select(e => new
            {
                e.Id,
                e.Title,
                e.Description,
                e.StartDate,
                e.EndDate,
                e.MaxParticipants,
                e.ImageUrl,
                e.IsPublic,
                CreatedBy = new
                {
                    e.CreatedByNavigation.Id,
                    e.CreatedByNavigation.FirstName,
                    e.CreatedByNavigation.LastName
                }
            })
            .ToListAsync();
    }

    public async Task RegisterForEvent(EventRegistrationRequest request, int userId)
    {
        var eventItem = await context.Events.FindAsync(request.EventId);

        if (eventItem == null) throw new Exception("Event not found");

        // TODO: (maybe)
        // Check if user is already registered for the event
        // Check if the event is full
    }
}