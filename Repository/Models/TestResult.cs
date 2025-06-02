using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class TestResult
{
    public int TestId { get; set; }

    public string DonorId { get; set; } = null!;

    public DateOnly TestDate { get; set; }

    public string? ResultNote { get; set; }

    public virtual Donor Donor { get; set; } = null!;
}
