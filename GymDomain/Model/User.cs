using System.ComponentModel.DataAnnotations.Schema;

namespace GymDomain.Model;

[Table("Users")]
public partial class User: Entity
{
    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;
    public string Role { get; set; } = "User"; 
    public virtual ICollection<NutritionPlan> NutritionPlans { get; set; } = new List<NutritionPlan>();

    public virtual ICollection<ProgressTracking> ProgressTrackings { get; set; } = new List<ProgressTracking>();

    public virtual ICollection<TrainingPlan> TrainingPlans { get; set; } = new List<TrainingPlan>();
}
