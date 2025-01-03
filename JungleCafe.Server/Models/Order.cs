namespace JungleCafe.Server.Models;

public class Order
{
    public int Id { get; set; }

    public int ReservationId { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual Reservation Reservation { get; set; } = null!;
}