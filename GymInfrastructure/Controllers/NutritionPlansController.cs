using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymDomain.Model;
using Microsoft.AspNetCore.Authorization;

namespace GymInfrastructure.Controllers
{
    [Authorize]
    public class NutritionPlansController : Controller
    {
        private readonly GYMDbContext _context;

        public NutritionPlansController(GYMDbContext context)
        {
            _context = context;
        }

        // GET: NutritionPlans
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var plans = await _context.NutritionPlans
                .Include(n => n.User)
                .Where(n => n.UserId == currentUser.Id)
                .ToListAsync();

            return View(plans);
        }

        // GET: NutritionPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var nutritionPlan = await _context.NutritionPlans
                .Include(n => n.User)
                .Include(n => n.NutritionPlanMeals)
                .ThenInclude(npm => npm.Meals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (nutritionPlan == null)
            {
                return NotFound();
            }

            return View(nutritionPlan);
        }

        public IActionResult Create()
        {
            return View();
        }


// POST: NutritionPlans/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NutritionPlan plan)
        {
            var userEmail = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            plan.UserId = currentUser.Id;
            _context.NutritionPlans.Add(plan);
            await _context.SaveChangesAsync();

            return RedirectToAction("SelectMeals", new { id = plan.Id });
        }


        // GET: NutritionPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var plan = await _context.NutritionPlans
                .Include(np => np.NutritionPlanMeals)
                .FirstOrDefaultAsync(np => np.Id == id);

            if (plan == null) return NotFound();

            ViewBag.Meals = await _context.Meals.ToListAsync();
            ViewBag.SelectedMeals = plan.NutritionPlanMeals.Select(npm => npm.MealsId).ToList();

            return View(plan);
        }


        // POST: NutritionPlans/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, NutritionPlan plan, int[] SelectedMeals)
        {
            if (id != plan.Id)
            {
                return NotFound();
            }

            var planToUpdate = await _context.NutritionPlans
                .Include(np => np.NutritionPlanMeals)
                .FirstOrDefaultAsync(np => np.Id == id);

            if (planToUpdate == null)
            {
                return NotFound();
            }

            planToUpdate.Name = plan.Name;
            planToUpdate.Description = plan.Description;
            
            _context.NutritionPlanMeals.RemoveRange(planToUpdate.NutritionPlanMeals);

            foreach (var mealId in SelectedMeals)
            {
                planToUpdate.NutritionPlanMeals.Add(new NutritionPlanMeal
                {
                    NutritionPlanId = plan.Id,
                    MealsId = mealId,
                    Quantity = 1
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: NutritionPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var plan = await _context.NutritionPlans
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (plan == null)
            {
                return NotFound();
            }

            return View(plan);
        }

        // POST: NutritionPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var plan = await _context.NutritionPlans.FindAsync(id);
            if (plan != null)
            {
                _context.NutritionPlans.Remove(plan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: NutritionPlans/SelectMeals/5
        public async Task<IActionResult> SelectMeals(int id)
        {
            var nutritionPlan = await _context.NutritionPlans.FindAsync(id);
            if (nutritionPlan == null)
            {
                return NotFound();
            }

            ViewBag.Meals = await _context.Meals.ToListAsync();
            ViewBag.NutritionPlanId = id;
            ViewBag.SelectedMeals = nutritionPlan.NutritionPlanMeals.Select(npm => npm.MealsId).ToList(); // додати!

            return View();
        }


// POST: NutritionPlans/SelectMeals/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectMeals(int id, int[] SelectedMeals)
        {
            var nutritionPlan = await _context.NutritionPlans.FindAsync(id);
            if (nutritionPlan == null)
            {
                return NotFound();
            }

            foreach (var mealId in SelectedMeals)
            {
                _context.NutritionPlanMeals.Add(new NutritionPlanMeal
                {
                    NutritionPlanId = id,
                    MealsId = mealId
                });
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
