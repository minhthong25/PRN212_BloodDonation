using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class BloodGroup
{
    public int BloodGroupId { get; set; }

    public string? GroupName { get; set; }

    public virtual ICollection<BloodInventory> BloodInventories { get; set; } = new List<BloodInventory>();

    public virtual ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();

    public virtual ICollection<Donor> Donors { get; set; } = new List<Donor>();
}
