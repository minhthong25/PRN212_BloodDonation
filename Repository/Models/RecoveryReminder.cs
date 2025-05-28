using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class RecoveryReminder
{
    public int ReminderId { get; set; }

    public int? DonorId { get; set; }

    public DateOnly? NextEligibleDate { get; set; }

    public bool? Notified { get; set; }

    public virtual Donor? Donor { get; set; }
}
