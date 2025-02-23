using System;
using System.Collections.Generic;

namespace lab1GYM.Model;

public partial class NutritionPlanMeal: entity
{
    public int NutritionPlanId { get; set; }

    public int Quantity { get; set; }

    public virtual Meal Meals { get; set; } = null!;

    public virtual NutritionPlan NutritionPlan { get; set; } = null!;
}
