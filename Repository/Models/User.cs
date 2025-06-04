using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class User
{
    public int UserId { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Phone { get; set; }

    public string Role { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Donor? Donor { get; set; }

    public virtual Recipient? Recipient { get; set; }

    public virtual ICollection<RequestApproval> RequestApprovals { get; set; } = new List<RequestApproval>();
}
