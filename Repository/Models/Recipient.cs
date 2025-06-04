using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Recipient
{
    public int RecipientId { get; set; }

    public string? MedicalCondition { get; set; }

    public virtual ICollection<BloodRequest> BloodRequests { get; set; } = new List<BloodRequest>();

    public virtual User RecipientNavigation { get; set; } = null!;
}
