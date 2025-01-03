namespace JungleCafe.Server.Models;

public class OrderItem
{
    public int OrderId { get; set; }

    public int MenuItemId { get; set; }

    public int Quantity { get; set; }

    public string Notes { get; set; } = null!;

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}