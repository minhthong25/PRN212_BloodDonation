using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public virtual ICollection<RequestApproval> RequestApprovals { get; set; } = new List<RequestApproval>();

    public virtual User StaffNavigation { get; set; } = null!;
}
