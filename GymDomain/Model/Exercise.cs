namespace GymDomain.Model;

public partial class Exercise: Entity
{
    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;
    public string? ImagePath { get; set; }
    
    public string? ReferenceUrl { get; set; }


    
    public virtual ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; } = new List<TrainingPlanExercise>();
}
