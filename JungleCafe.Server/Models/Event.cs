﻿using Microsoft.EntityFrameworkCore;

namespace JungleCafe.Server.Models;

public class Event
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    [Precision(0)]
    public DateTime StartDate { get; set; }

    [Precision(0)]
    public DateTime EndDate { get; set; }

    public int? MaxParticipants { get; set; }

    public string? ImageUrl { get; set; }

    public bool IsPublic { get; set; }

    public int CreatedBy { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;
}