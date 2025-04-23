using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymDomain.Model;
using Microsoft.AspNetCore.Authorization;


namespace GymInfrastructure.Controllers
{
    [Authorize]
    public class PhotoEntriesController : Controller
    {
        private readonly GYMDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PhotoEntriesController(GYMDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: PhotoEntries
        public async Task<IActionResult> Index()
        {
            var userEmail = User.Identity.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
                return RedirectToAction("Login", "Account");

            var photos = await _context.PhotoEntries
                .Where(p => p.UserId == currentUser.Id)
                .ToListAsync();

            return View(photos);
        }

        // GET: PhotoEntries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photoEntry = await _context.PhotoEntries
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photoEntry == null)
            {
                return NotFound();
            }

            return View(photoEntry);
        }

        // GET: PhotoEntries/Create
        public IActionResult Create()
        { 
            return View();
        }

        // POST: PhotoEntries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DateTime date, IFormFile photo)
        {
            var userEmail = User.Identity?.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (photo != null && photo.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(photo.FileName);
                var filePath = Path.Combine(_env.WebRootPath, "uploads", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await photo.CopyToAsync(stream);
                }

                var photoEntry = new PhotoEntry
                {
                    UserId = user.Id,
                    Date = date,
                    PhotoPath = "/uploads/" + fileName
                };

                _context.PhotoEntries.Add(photoEntry);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ModelState.AddModelError("", "Please select a photo.");
            return View();
        }

        // GET: PhotoEntries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photoEntry = await _context.PhotoEntries.FindAsync(id);
            if (photoEntry == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", photoEntry.UserId);
            return View(photoEntry);
        }

        // POST: PhotoEntries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PhotoPath,Date,Id")] PhotoEntry photoEntry)
        {
            if (id != photoEntry.Id)
            {
                return NotFound();
            }
            var existing = await _context.PhotoEntries.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            if (existing == null)
            {
                return NotFound();
            }

            photoEntry.UserId = existing.UserId;

            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(photoEntry);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PhotoEntryExists(photoEntry.Id))
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

            return View(photoEntry);
        }


        // GET: PhotoEntries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var photoEntry = await _context.PhotoEntries
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (photoEntry == null)
            {
                return NotFound();
            }

            return View(photoEntry);
        }

        // POST: PhotoEntries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var photoEntry = await _context.PhotoEntries.FindAsync(id);
            if (photoEntry != null)
            {
                _context.PhotoEntries.Remove(photoEntry);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PhotoEntryExists(int id)
        {
            return _context.PhotoEntries.Any(e => e.Id == id);
        }
    }
}
