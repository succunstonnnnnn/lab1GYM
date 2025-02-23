using System;
using System.Collections.Generic;

namespace lab1GYM.Model;

public partial class User: entity
{

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<NutritionPlan> NutritionPlans { get; set; } = new List<NutritionPlan>();

    public virtual ICollection<ProgressTracking> ProgressTrackings { get; set; } = new List<ProgressTracking>();

    public virtual ICollection<TrainingPlan> TrainingPlans { get; set; } = new List<TrainingPlan>();
}
