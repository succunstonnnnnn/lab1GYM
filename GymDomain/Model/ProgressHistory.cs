namespace GymDomain.Model;

public class ProgressHistory : Entity
{
    public int UserId { get; set; }
    public string Note { get; set; } = null!;
    public DateTime Date { get; set; }

    public virtual User User { get; set; } = null!;
}