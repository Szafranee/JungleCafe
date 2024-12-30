using System;
using System.Collections.Generic;

namespace JungleCafe.Server.Models;

public partial class Table
{
    public int Id { get; set; }

    public string Number { get; set; } = null!;

    public int Capacity { get; set; }

    public string Zone { get; set; } = null!;

    public bool IsAvailable { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
