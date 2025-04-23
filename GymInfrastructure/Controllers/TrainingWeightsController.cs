using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymDomain.Model;

namespace GymInfrastructure.Controllers
{
    public class TrainingWeightsController : Controller
    {
        private readonly GYMDbContext _context;

        public TrainingWeightsController(GYMDbContext context)
        {
            _context = context;
        }

        // GET: TrainingWeights
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var weights = await _context.TrainingWeights
                .Include(w => w.Exercise)
                .Where(w => w.UserId == currentUser.Id)
                .ToListAsync();


            return View(weights);
        }


        // GET: TrainingWeights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingWeight = await _context.TrainingWeights
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingWeight == null)
            {
                return NotFound();
            }

            return View(trainingWeight);
        }

        // GET: TrainingWeights/Create
        public IActionResult Create()
        {
            var exercises = _context.Exercises
                .Select(e => new SelectListItem
                {
                    Value = e.Id.ToString(),
                    Text = e.Name
                })
                .ToList();

            ViewBag.Exercises = exercises;

            return View();
        }
        // POST: TrainingWeights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("UserId,ExerciseName,Weight,Date,Id")] TrainingWeight trainingWeight)
        {
            {
                var userEmail = User.Identity?.Name;
                var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

                if (currentUser == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                trainingWeight.UserId = currentUser.Id;
                ModelState.Remove("User");

                if (ModelState.IsValid)
                {
                    _context.TrainingWeights.Add(trainingWeight);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(trainingWeight);
        }
        
        [HttpGet]
        public JsonResult GetExerciseNames(string term)
        {
            var names = _context.Exercises
                .Where(e => e.Name.Contains(term))
                .Select(e => e.Name)
                .Distinct()
                .ToList();

            return Json(names);
        }

        // GET: TrainingWeights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingWeight = await _context.TrainingWeights.FindAsync(id);
            if (trainingWeight == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", trainingWeight.UserId);
            return View(trainingWeight);
        }

        // POST: TrainingWeights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExerciseName,Weight,Date,Id")] TrainingWeight trainingWeight)
        {
            if (id != trainingWeight.Id)
            {
                return NotFound();
            }

            // Витягуємо UserId з БД (оскільки воно не в формі, але потрібне для збереження)
            var existing = await _context.TrainingWeights.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            trainingWeight.UserId = existing.UserId;

            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingWeight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingWeightExists(trainingWeight.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(trainingWeight);
        }


        // GET: TrainingWeights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingWeight = await _context.TrainingWeights
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingWeight == null)
            {
                return NotFound();
            }

            return View(trainingWeight);
        }

        // POST: TrainingWeights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingWeight = await _context.TrainingWeights.FindAsync(id);
            if (trainingWeight != null)
            {
                _context.TrainingWeights.Remove(trainingWeight);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingWeightExists(int id)
        {
            return _context.TrainingWeights.Any(e => e.Id == id);
        }
    }
}
