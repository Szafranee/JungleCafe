using JungleCafe.Server.DTOs;
using JungleCafe.Server.Models;
using JungleCafe.Server.RequestModels;
using JungleCafe.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Services;

public class EventsService(CafeDbContext context) : IEventService
{
    public async Task<IEnumerable<EventDto>> GetEvents()
    {
        var events = await context.Events
            .Include(e => e.CreatedByNavigation)
            .Select(e => new EventDto
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                StartDate = e.StartDate,
                EndDate = e.EndDate,
                MaxParticipants = e.MaxParticipants,
                ImageUrl = e.ImageUrl,
                IsPublic = e.IsPublic,
                Creator = new CreatorDto
                {
                    Id = e.CreatedByNavigation.Id,
                    FirstName = e.CreatedByNavigation.FirstName,
                    LastName = e.CreatedByNavigation.LastName,
                    Email = e.CreatedByNavigation.Email
                }
            })
            .ToListAsync();

        return events;
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

    public async Task<EventDto> CreateEvent(Event eventItem)
    {
        context.Events.Add(eventItem);
        await context.SaveChangesAsync();

        var user = await context.Users.FindAsync(eventItem.CreatedBy);

        return new EventDto
        {
            Id = eventItem.Id,
            Title = eventItem.Title,
            Description = eventItem.Description,
            StartDate = eventItem.StartDate,
            EndDate = eventItem.EndDate,
            MaxParticipants = eventItem.MaxParticipants,
            ImageUrl = eventItem.ImageUrl,
            IsPublic = eventItem.IsPublic,
            Creator = new CreatorDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email
            }
        };
    }

    public async Task<Event?> UpdateEvent(int id, EventUpdateDto eventDto)
    {
        var existingEvent = await context.Events.FindAsync(id);
        if (existingEvent == null)
        {
            return null;
        }

        existingEvent.Title = eventDto.Title;
        existingEvent.Description = eventDto.Description;
        existingEvent.StartDate = eventDto.StartDate;
        existingEvent.EndDate = eventDto.EndDate;
        existingEvent.MaxParticipants = eventDto.MaxParticipants;
        existingEvent.ImageUrl = eventDto.ImageUrl;
        existingEvent.IsPublic = eventDto.IsPublic;

        await context.SaveChangesAsync();
        return existingEvent;
    }

    public async Task DeleteEvent(int id)
    {
        var eventItem = await context.Events.FindAsync(id);
        if (eventItem == null)
        {
            throw new Exception("Event not found");
        }

        context.Events.Remove(eventItem);
        await context.SaveChangesAsync();
    }
}