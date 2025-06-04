using System;
using System.Collections.Generic;

namespace Repository.Models;

public partial class TestResult
{
    public int TestId { get; set; }

    public int DonorId { get; set; }

    public DateOnly TestDate { get; set; }

    public string? ResultNote { get; set; }

    public virtual Donor Donor { get; set; } = null!;
}
