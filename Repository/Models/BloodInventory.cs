using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class BloodInventory
{
    public int InventoryId { get; set; }

    public int BloodGroupId { get; set; }

    public int Quantity { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual BloodGroup BloodGroup { get; set; } = null!;
}
