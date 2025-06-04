using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Donor
{
    public int DonorId { get; set; }

    public int BloodGroupId { get; set; }

    public DateOnly? LastDonationDate { get; set; }

    public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();

    public virtual BloodGroup BloodGroup { get; set; } = null!;

    public virtual User DonorNavigation { get; set; } = null!;

    public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
}
