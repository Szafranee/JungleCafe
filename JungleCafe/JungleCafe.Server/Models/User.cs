using System;
using System.Collections.Generic;

namespace JungleCafe.Server.Models;

public partial class User
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
