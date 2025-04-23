using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GymInfrastructure.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly GYMDbContext _context;

        public AdminController(GYMDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var topExercises = _context.TrainingPlanExercises
                .GroupBy(e => e.Exercises.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            ViewBag.ExerciseLabels = topExercises.Select(e => e.Name).ToList();
            ViewBag.ExerciseValues = topExercises.Select(e => e.Count).ToList();

            var topMeals = _context.NutritionPlanMeals
                .GroupBy(m => m.Meals.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .Take(5)
                .ToList();

            ViewBag.MealLabels = topMeals.Select(m => m.Name).ToList();
            ViewBag.MealValues = topMeals.Select(m => m.Count).ToList();

            return View();
        }


        public async Task<IActionResult> UserList()
        {
            var users = await _context.Users.ToListAsync();
            return View(users);
        }
    }
}