using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class BloodRequest
{
    public int RequestId { get; set; }

    public int RecipientId { get; set; }

    public int BloodGroupId { get; set; }

    public int Quantity { get; set; }

    public string Status { get; set; } = null!;

    public string? ResponseMessage { get; set; }

    public DateTime RequestDate { get; set; }

    public virtual BloodGroup BloodGroup { get; set; } = null!;

    public virtual Recipient Recipient { get; set; } = null!;

    public virtual ICollection<RequestApproval> RequestApprovals { get; set; } = new List<RequestApproval>();
}
