using System;
using System.Collections.Generic;

namespace lab1GYM.Model;

public partial class Meal: entity
{
    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Calories { get; set; }

    public double Protein { get; set; }

    public double Fats { get; set; }

    public virtual ICollection<NutritionPlanMeal> NutritionPlanMeals { get; set; } = new List<NutritionPlanMeal>();
}
