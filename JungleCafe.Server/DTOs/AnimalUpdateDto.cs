namespace JungleCafe.Server.DTOs;

public class AnimalUpdateDto
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Species { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int CaretakerId { get; set; }
}