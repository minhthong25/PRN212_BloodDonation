using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string? Name { get; set; }

    public string? Address { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
