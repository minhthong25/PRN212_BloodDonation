using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class RequestApproval
{
    public int ApprovalId { get; set; }

    public int? RequestId { get; set; }

    public int? StaffId { get; set; }

    public string? ApprovalStatus { get; set; }

    public string? Notes { get; set; }

    public DateTime? ApprovalDate { get; set; }

    public virtual BloodRequest? Request { get; set; }

    public virtual Staff? Staff { get; set; }
}
