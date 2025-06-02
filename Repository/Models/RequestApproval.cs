using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class RequestApproval
{
    public int ApprovalId { get; set; }

    public int RequestId { get; set; }

    public string ApproverUserId { get; set; } = null!;

    public string ApprovalStatus { get; set; } = null!;

    public string? Notes { get; set; }

    public DateTime ApprovalDate { get; set; }

    public virtual User ApproverUser { get; set; } = null!;

    public virtual BloodRequest Request { get; set; } = null!;
}
