namespace GymDomain.Model;

public partial class TrainingPlan: Entity
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; } = new List<TrainingPlanExercise>();

    public virtual User User { get; set; } = null!;
}
