namespace GymDomain.Model;

public partial class Meal: Entity
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Calories { get; set; }

    public double Protein { get; set; }

    public double Fats { get; set; }
    public string? ImagePath { get; set; }
    public string? ReferenceUrl { get; set; }


    public virtual ICollection<NutritionPlanMeal> NutritionPlanMeals { get; set; } = new List<NutritionPlanMeal>();
}
