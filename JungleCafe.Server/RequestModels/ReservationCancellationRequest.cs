namespace JungleCafe.Server.RequestModels;

public class ReservationCancellationRequest
{
    public int ReservationId { get; set; }
    public string Reason { get; set; }
}