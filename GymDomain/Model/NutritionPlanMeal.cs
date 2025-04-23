namespace GymDomain.Model;

public partial class NutritionPlanMeal
{
    public int NutritionPlanId { get; set; }

    public int MealsId { get; set; }

    public int Quantity { get; set; }

    public virtual Meal Meals { get; set; } = null!;

    public virtual NutritionPlan NutritionPlan { get; set; } = null!;
}
