namespace GymDomain.Model;

public partial class NutritionPlan: Entity
{
    public int UserId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<NutritionPlanMeal> NutritionPlanMeals { get; set; } = new List<NutritionPlanMeal>();

    public virtual User User { get; set; } = null!;
}
