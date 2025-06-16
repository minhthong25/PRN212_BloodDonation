using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Location
{
    public int LocationId { get; set; }

    public string Name { get; set; } = null!;

    public string Address { get; set; } = null!;
    public DateTime? EventDate { get; set; }  // thêm event date

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
}
