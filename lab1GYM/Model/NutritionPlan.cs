using System;
using System.Collections.Generic;

namespace lab1GYM.Model;

public partial class NutritionPlan: entity
{
    public int NutritionPlanId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<NutritionPlanMeal> NutritionPlanMeals { get; set; } = new List<NutritionPlanMeal>();

    public virtual User User { get; set; } = null!;
}
