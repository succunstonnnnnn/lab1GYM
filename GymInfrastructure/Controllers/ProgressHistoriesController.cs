using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymDomain.Model;

namespace GymInfrastructure.Controllers
{
    public class ProgressHistoriesController : Controller
    {
        private readonly GYMDbContext _context;

        public ProgressHistoriesController(GYMDbContext context)
        {
            _context = context;
        }

        // GET: ProgressHistories
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var history = await _context.ProgressHistories
                .Where(h => h.UserId == currentUser.Id)
                .ToListAsync();

            return View(history);
        }

        // GET: ProgressHistories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progressHistory = await _context.ProgressHistories
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (progressHistory == null)
            {
                return NotFound();
            }

            return View(progressHistory);
        }

        // GET: ProgressHistories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProgressHistories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Note,Date,Id")] ProgressHistory progressHistory)
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            progressHistory.UserId = currentUser.Id;
            ModelState.Remove("User");
            
            if (ModelState.IsValid)
            {
                _context.ProgressHistories.Add(progressHistory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(progressHistory);
        }

        // GET: ProgressHistories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progressHistory = await _context.ProgressHistories.FindAsync(id);
            if (progressHistory == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", progressHistory.UserId);
            return View(progressHistory);
        }

        // POST: ProgressHistories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Note,Date,Id")] ProgressHistory progressHistory)
        {
            if (id != progressHistory.Id)
            {
                return NotFound();
            }

            // Витягуємо UserId з БД (бо він required через foreign key)
            var existing = await _context.ProgressHistories.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            progressHistory.UserId = existing.UserId;

            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(progressHistory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.ProgressHistories.Any(e => e.Id == progressHistory.Id))
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

            return View(progressHistory);
        }

        // GET: ProgressHistories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progressHistory = await _context.ProgressHistories
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (progressHistory == null)
            {
                return NotFound();
            }

            return View(progressHistory);
        }

        // POST: ProgressHistories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var progressHistory = await _context.ProgressHistories.FindAsync(id);
            if (progressHistory != null)
            {
                _context.ProgressHistories.Remove(progressHistory);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgressHistoryExists(int id)
        {
            return _context.ProgressHistories.Any(e => e.Id == id);
        }
    }
}
