using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymDomain.Model;
using Microsoft.AspNetCore.Authorization;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;

namespace GymInfrastructure.Controllers
{
    [Authorize]
    public class BodyParametersController : Controller
    {
        private readonly GYMDbContext _context;

        public BodyParametersController(GYMDbContext context)
        {
            _context = context;
        }


 [HttpPost]
public async Task<IActionResult> ExportToPdf(DateTime startDate, DateTime endDate)
{
    var userEmail = User.Identity.Name;
    var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
    if (user == null) return RedirectToAction("Login", "Account");

    var parameters = await _context.BodyParameters
        .Where(p => p.UserId == user.Id && p.Date >= startDate && p.Date <= endDate)
        .OrderBy(p => p.Date)
        .ToListAsync();

    using var ms = new MemoryStream();
    var doc = new PdfDocument();
    var page = doc.AddPage();
    var gfx = XGraphics.FromPdfPage(page);

    var fontRegular = new XFont("Arial", 12, XFontStyle.Regular);
    var fontBold = new XFont("Arial", 12, XFontStyle.Bold);
    var fontTitle = new XFont("Arial", 18, XFontStyle.Bold);

    double margin = 40;
    double y = margin;
    
    gfx.DrawString("WEIGHT/CIRCUMFERENCES TRACKER", fontTitle, XBrushes.Black,
        new XRect(0, y, page.Width, 40), XStringFormats.TopCenter);
    y += 40;
    
    gfx.DrawString($"Start Date: {startDate:yyyy-MM-dd}", fontRegular, XBrushes.Black,
        new XRect(margin, y, page.Width / 2 - margin, 20), XStringFormats.TopLeft);
    gfx.DrawString($"End Date: {endDate:yyyy-MM-dd}", fontRegular, XBrushes.Black,
        new XRect(page.Width / 2, y, page.Width / 2 - margin, 20), XStringFormats.TopRight);
    y += 25;


    gfx.DrawString("Goal Weight: ____________________", fontRegular, XBrushes.Black,
        new XRect(margin, y, page.Width - margin * 2, 20), XStringFormats.TopLeft);
    y += 35;
    
    string[] headers = { "Date", "Weight", "Waist", "Hips", "Arms", "Thighs", "Chest" };
    double[] columnWidths = { 90, 60, 60, 60, 60, 60, 60 };
    double tableWidth = columnWidths.Sum();
    double x = (page.Width - tableWidth) / 2;
    
    for (int i = 0; i < headers.Length; i++)
    {
        var isBold = headers[i] == "Date" || headers[i] == "Weight";
        gfx.DrawRectangle(XPens.LightGray, XBrushes.WhiteSmoke,
            new XRect(x, y, columnWidths[i], 25));
        gfx.DrawString(headers[i],
            isBold ? fontBold : fontRegular, XBrushes.Black,
            new XRect(x, y + 5, columnWidths[i], 20), XStringFormats.TopCenter);
        x += columnWidths[i];
    }

    y += 25;
    
    foreach (var p in parameters)
    {
        string[] row = {
            p.Date.ToString("yyyy-MM-dd"),
            p.Weight.ToString(),
            p.Waist?.ToString() ?? "",
            p.Glutes?.ToString() ?? "",
            p.Biceps?.ToString() ?? "",
            p.Thigh?.ToString() ?? "",
            p.Chest?.ToString() ?? ""
        };

        x = (page.Width - tableWidth) / 2;
        for (int i = 0; i < row.Length; i++)
        {
            gfx.DrawRectangle(new XPen(XColors.LightGray, 0.5), new XSolidBrush(XColors.White),
                new XRect(x, y, columnWidths[i], 20));
            gfx.DrawString(row[i], fontRegular, XBrushes.Black,
                new XRect(x, y + 4, columnWidths[i], 20), XStringFormats.TopCenter);
            x += columnWidths[i];
        }
        y += 20;
        
        if (y > page.Height - 40)
        {
            page = doc.AddPage();
            gfx = XGraphics.FromPdfPage(page);
            y = margin;
        }
    }

    doc.Save(ms, false);
    return File(ms.ToArray(), "application/pdf", "Weight_Loss_Tracker.pdf");
}


        // GET: BodyParameters
        public async Task<IActionResult> Index(DateTime? startDate, DateTime? endDate)
        {
            var userEmail = User.Identity.Name;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return RedirectToAction("Login", "Account");

            var query = _context.BodyParameters.Where(p => p.UserId == user.Id);

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(p => p.Date >= startDate && p.Date <= endDate);
            }

            var parameters = await query.OrderBy(p => p.Date).ToListAsync();

            ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
            ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");

            return View(parameters);
        }

        // GET: BodyParameters/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyParameter = await _context.BodyParameters
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bodyParameter == null)
            {
                return NotFound();
            }

            return View(bodyParameter);
        }
        
        // GET: BodyParameters/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: BodyParameters/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // POST: BodyParameters/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,Weight,Waist,Chest,Thigh,Biceps,Calf,Glutes,TargetWeight")] BodyParameter bodyParameter)
        {
            var userEmail = User.Identity?.Name;
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userEmail);

            if (currentUser == null)
            {
                return RedirectToAction("Login", "Account");
            }

            bodyParameter.UserId = currentUser.Id;
            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                _context.BodyParameters.Add(bodyParameter);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    Console.WriteLine($"Validation error in '{state.Key}': {error.ErrorMessage}");
                }
            }

            return View(bodyParameter);
        }



        // GET: BodyParameters/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyParameter = await _context.BodyParameters.FindAsync(id);
            if (bodyParameter == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", bodyParameter.UserId);
            return View(bodyParameter);
        }

        // POST: BodyParameters/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,Weight,TargetWeight,Waist,Chest,Thigh,Biceps,Calf,Glutes")] BodyParameter bodyParameter)
        {
            if (id != bodyParameter.Id)
            {
                return NotFound();
            }

            ModelState.Remove("User");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.BodyParameters.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                    if (existing == null)
                        return NotFound();
                    
                    bodyParameter.UserId = existing.UserId;

                    _context.Update(bodyParameter);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BodyParameterExists(bodyParameter.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            return View(bodyParameter);
        }


        // GET: BodyParameters/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bodyParameter = await _context.BodyParameters
                .Include(b => b.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bodyParameter == null)
            {
                return NotFound();
            }

            return View(bodyParameter);
        }

        // POST: BodyParameters/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bodyParameter = await _context.BodyParameters.FindAsync(id);
            if (bodyParameter != null)
            {
                _context.BodyParameters.Remove(bodyParameter);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BodyParameterExists(int id)
        {
            return _context.BodyParameters.Any(e => e.Id == id);
        }
    }
}
