namespace JungleCafe.Server.DTOs;

public class ReservationDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public int TableId { get; set; }
    public string TableNumber { get; set; }
    public DateTime ReservationDate { get; set; }
    public int Duration { get; set; }
    public string Status { get; set; }
    public int GuestCount { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CancellationReason { get; set; }
}