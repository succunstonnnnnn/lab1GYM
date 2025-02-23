using System;
using System.Collections.Generic;

namespace lab1GYM.Model;

public partial class Exercise: entity
{
    public int ExercisesId { get; set; }

    public int TypeId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<TrainingPlanExercise> TrainingPlanExercises { get; set; } = new List<TrainingPlanExercise>();
}
