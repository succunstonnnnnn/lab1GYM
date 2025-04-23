namespace GymDomain.Model;

public class PhotoEntry : Entity
{
    public int UserId { get; set; }
    public string PhotoPath { get; set; } = null!;
    public DateTime Date { get; set; }

    public virtual User User { get; set; } = null!;
}