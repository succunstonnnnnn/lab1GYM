using System;
using System.Collections.Generic;

namespace lab1GYM.Model;

public partial class TrainingPlan: entity
{

    public int Duration { get; set; }

    public int? SetsReps { get; set; }

    public virtual ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; } = new List<TrainingPlanExercise>();

    public virtual User User { get; set; } = null!;
}
