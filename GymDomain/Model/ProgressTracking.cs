namespace GymDomain.Model;

public partial class ProgressTracking: Entity
{
    public int UserId { get; set; }

    public DateOnly Date { get; set; }

    public double Weight { get; set; }

    public string? Circumferences { get; set; }

    public virtual User User { get; set; } = null!;
}
