namespace GymDomain.Model;

public partial class TrainingPlanExercise
{
    public int TrainingPlanId { get; set; }

    public int ExercisesId { get; set; }

    public int Sets { get; set; }

    public int Reps { get; set; }

    public virtual Exercise Exercises { get; set; } = null!;

    public virtual TrainingPlan TrainingPlan { get; set; } = null!;
}
