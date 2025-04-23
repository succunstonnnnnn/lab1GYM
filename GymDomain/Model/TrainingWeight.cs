namespace GymDomain.Model;

public class TrainingWeight : Entity
{
    public int UserId { get; set; }
    public string ExerciseName { get; set; } = null!;
    public float Weight { get; set; }
    public DateTime Date { get; set; }
    public int? ExerciseId { get; set; }
    public virtual Exercise? Exercise { get; set; }

    public virtual User User { get; set; } = null!;
}