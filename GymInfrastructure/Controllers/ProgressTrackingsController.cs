using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymDomain.Model;
using Microsoft.AspNetCore.Authorization;

namespace GymInfrastructure.Controllers
{
    [Authorize]
    public class ProgressTrackingsController : Controller
    {
        private readonly GYMDbContext _context;

        public ProgressTrackingsController(GYMDbContext context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var userEmail = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (currentUser == null) return RedirectToAction("Login", "Account");

            var progress = await _context.ProgressTrackings
                .Include(p => p.User)
                .Where(p => p.UserId == currentUser.Id)
                .ToListAsync();

            var bodyQuery = _context.BodyParameters
                .Where(x => x.UserId == currentUser.Id && x.Weight != null);

            if (startDate.HasValue && endDate.HasValue)
            {
                bodyQuery = bodyQuery.Where(x => x.Date >= startDate && x.Date <= endDate);
            }

            var bodyData = await bodyQuery.OrderBy(x => x.Date).ToListAsync();
            ViewBag.WeightLabels = bodyData.Select(b => b.Date.ToString("dd.MM")).ToList();
            ViewBag.WeightValues = bodyData.Select(b => b.Weight).ToList();

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
            
            ViewBag.LastHistory = await _context.ProgressHistories
                .Where(x => x.UserId == currentUser.Id)
                .OrderByDescending(x => x.Date)
                .Take(3)
                .ToListAsync();

            ViewBag.LastPhoto = await _context.PhotoEntries
                .Where(x => x.UserId == currentUser.Id)
                .OrderByDescending(x => x.Date)
                .FirstOrDefaultAsync();

            ViewBag.LastWeights = await _context.TrainingWeights
                .Include(x => x.Exercise)
                .Where(x => x.UserId == currentUser.Id)
                .OrderByDescending(x => x.Date)
                .Take(3)
                .ToListAsync();
            
            return View(progress);
        }

        

        // GET: ProgressTrackings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progressTracking = await _context.ProgressTrackings
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (progressTracking == null)
            {
                return NotFound();
            }

            return View(progressTracking);
        }

        // GET: ProgressTrackings/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: ProgressTrackings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("UserId,Date,Weight,Circumferences,Id")] ProgressTracking progressTracking)
        {
            if (ModelState.IsValid)
            {
                _context.Add(progressTracking);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", progressTracking.UserId);
            return View(progressTracking);
        }

        // GET: ProgressTrackings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progressTracking = await _context.ProgressTrackings.FindAsync(id);
            if (progressTracking == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", progressTracking.UserId);
            return View(progressTracking);
        }

        // POST: ProgressTrackings/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Date,Weight,Circumferences,Id")] ProgressTracking progressTracking)
        {
            if (id != progressTracking.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(progressTracking);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProgressTrackingExists(progressTracking.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", progressTracking.UserId);
            return View(progressTracking);
        }

        // GET: ProgressTrackings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var progressTracking = await _context.ProgressTrackings
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (progressTracking == null)
            {
                return NotFound();
            }

            return View(progressTracking);
        }

        // POST: ProgressTrackings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var progressTracking = await _context.ProgressTrackings.FindAsync(id);
            if (progressTracking != null)
            {
                _context.ProgressTrackings.Remove(progressTracking);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProgressTrackingExists(int id)
        {
            return _context.ProgressTrackings.Any(e => e.Id == id);
        }
    }
}
