using System;
using System.Collections.Generic;

namespace JungleCafe.Server.Models;

public partial class OrderItem
{
    public int OrderId { get; set; }

    public int MenuItemId { get; set; }

    public int Quantity { get; set; }

    public string Notes { get; set; } = null!;

    public virtual MenuItem MenuItem { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;
}
