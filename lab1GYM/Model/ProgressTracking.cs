using System;
using System.Collections.Generic;

namespace lab1GYM.Model;

public partial class ProgressTracking: entity
{
    public DateOnly Date { get; set; }

    public double Weight { get; set; }

    public string? Circumferences { get; set; }

    public virtual User User { get; set; } = null!;
}
