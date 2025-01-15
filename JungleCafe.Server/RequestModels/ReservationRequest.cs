namespace JungleCafe.Server.RequestModels;

public class ReservationRequest
{
    public int TableId { get; set; }
    public DateTime ReservationDate { get; set; }
    public int Duration { get; set; }
    public int GuestCount { get; set; }
    public string? SpecialRequests { get; set; }
}