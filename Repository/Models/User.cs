using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Donor? Donor { get; set; }

    public virtual Recipient? Recipient { get; set; }

    public virtual Staff? Staff { get; set; }
}
