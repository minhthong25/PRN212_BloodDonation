using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Appointment
{
    public int AppointmentId { get; set; }

    public int? DonorId { get; set; }

    public int? LocationId { get; set; }

    public DateTime? AppointmentDate { get; set; }

    public bool? IsCompleted { get; set; }

    public virtual Donor? Donor { get; set; }

    public virtual Location? Location { get; set; }
}
