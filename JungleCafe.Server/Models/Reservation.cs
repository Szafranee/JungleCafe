namespace JungleCafe.Server.Models;

public class Reservation
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int TableId { get; set; }

    public DateTime ReservationDate { get; set; }

    public int Duration { get; set; }

    public string Status { get; set; } = null!;

    public int GuestsCount { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool IsCancelled { get; set; }

    public string? CancellationReason { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Table Table { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}