using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymDomain.Model;
using Microsoft.AspNetCore.Authorization;

namespace GymInfrastructure.Controllers
{
    [Authorize]
    public class TrainingPlansController : Controller
    {
        private readonly GYMDbContext _context;

        public TrainingPlansController(GYMDbContext context)
        {
            _context = context;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartTraining(int id)
        {
            HttpContext.Session.SetString("TrainingActive", "true");
            HttpContext.Session.SetInt32("TrainingPlanId", id);

            return RedirectToAction("Details", new { id });
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndTraining(int id)
        {
            var userEmail = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (user == null)
                return RedirectToAction("Login", "Account");

            var trainingPlan = await _context.TrainingPlans.FirstOrDefaultAsync(tp => tp.Id == id);

            if (trainingPlan == null)
                return NotFound();

            var note = $"Training is finished: {trainingPlan.Name}";

            var history = new ProgressHistory
            {
                UserId = user.Id,
                Date = DateTime.Now,
                Note = note
            };

            _context.ProgressHistories.Add(history);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Training is finished and added to your history.";
            return RedirectToAction("Details", new { id });
        }

        // GET: TrainingPlans
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var userPlans = await _context.TrainingPlans
                .Where(tp => tp.UserId == currentUser.Id)
                .Include(tp => tp.User)
                .ToListAsync();

            return View(userPlans);
        }

        // GET: TrainingPlans/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trainingPlan = await _context.TrainingPlans
                .Include(tp => tp.User)
                .Include(tp => tp.TrainingPlanExercises)
                .ThenInclude(tpe => tpe.Exercises)
                .FirstOrDefaultAsync(tp => tp.Id == id);

            if (trainingPlan == null) return NotFound();

            TempData.Keep("TrainingActive"); 
            return View(trainingPlan);
        }



        // GET: TrainingPlans/SelectExercises/5
        public async Task<IActionResult> SelectExercises(int? id)
        {
            if (id == null) return NotFound();

            var plan = await _context.TrainingPlans.Include(tp => tp.TrainingPlanExercises).FirstOrDefaultAsync(tp => tp.Id == id);
            if (plan == null) return NotFound();

            var exercises = await _context.Exercises.ToListAsync();
            ViewBag.Exercises = exercises;
            ViewBag.PlanId = plan.Id;

            return View(plan);
        }
        
        // GET: TrainingPlans/Create
        public async Task<IActionResult> Create()
        {
            var exercises = await _context.Exercises.ToListAsync();
            ViewBag.Exercises = new SelectList(exercises, "Id", "Name");
            return View();
        }

        // POST: TrainingPlans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TrainingPlan plan, int[] SelectedExercises)
        {
            var userEmail = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Прив’язуємо план до поточного користувача
            plan.UserId = currentUser.Id;
            _context.TrainingPlans.Add(plan);
            await _context.SaveChangesAsync();

            // Додаємо вправи до плану
            foreach (var exerciseId in SelectedExercises)
            {
                _context.TrainingPlanExercises.Add(new TrainingPlanExercise
                {
                    TrainingPlanId = plan.Id,
                    ExercisesId = exerciseId
                });
            }
            await _context.SaveChangesAsync();

            // Після створення — назад на список планів
            return RedirectToAction("SelectExercises", new { id = plan.Id });
        }
        
        // GET: TrainingPlans/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlan = await _context.TrainingPlans.FindAsync(id);
            if (trainingPlan == null)
            {
                return NotFound();
            }
            return View(trainingPlan);
        }


        // POST: TrainingPlans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] TrainingPlan trainingPlan)
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            trainingPlan.UserId = currentUser.Id;
            ModelState.Remove("User");

            if (id != trainingPlan.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingPlan);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingPlanExists(trainingPlan.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(trainingPlan);
        }


        // GET: TrainingPlans/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingPlan = await _context.TrainingPlans
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingPlan == null)
            {
                return NotFound();
            }

            return View(trainingPlan);
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelectExercises(int planId, int[] selectedExercises)
        {
            var plan = await _context.TrainingPlans.Include(tp => tp.TrainingPlanExercises).FirstOrDefaultAsync(tp => tp.Id == planId);
            if (plan == null) return NotFound();
            
            _context.TrainingPlanExercises.RemoveRange(plan.TrainingPlanExercises);

            foreach (var exerciseId in selectedExercises)
            {
                _context.TrainingPlanExercises.Add(new TrainingPlanExercise
                {
                    TrainingPlanId = plan.Id,
                    ExercisesId = exerciseId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: TrainingPlans/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingPlan = await _context.TrainingPlans.FindAsync(id);
            if (trainingPlan != null)
            {
                _context.TrainingPlans.Remove(trainingPlan);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingPlanExists(int id)
        {
            return _context.TrainingPlans.Any(e => e.Id == id);
        }
    }
}
