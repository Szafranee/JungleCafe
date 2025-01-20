namespace JungleCafe.Server.DTOs;

public class TableAvailabilityDto
{
    public int Id { get; set; }
    public string Number { get; set; } = null!;
    public int Capacity { get; set; }
    public string Zone { get; set; } = null!;
    public bool IsAvailable { get; set; }
}