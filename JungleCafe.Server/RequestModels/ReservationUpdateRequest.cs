namespace JungleCafe.Server.RequestModels;

public class ReservationUpdateRequest
{
    public DateTime ReservationDate { get; set; }
    public int GuestCount { get; set; }
    public string? Notes { get; set; }
}