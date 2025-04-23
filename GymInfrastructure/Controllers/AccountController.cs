using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using GymInfrastructure; // Підключаємо твій namespace для контексту
using GymDomain.Model;  

namespace GymInfrastructure.Controllers
{
    public class AccountController : Controller
    {
        private readonly GYMDbContext _context; 
        
        public AccountController(GYMDbContext context)
        {
            _context = context;
        }

        // Login GET
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role ?? "User") 
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt.");
            return View();
        }


        // Register GET
        [HttpGet]
        public IActionResult Register() => View();

// Register POST
        [HttpPost]
        public async Task<IActionResult> Register(string name, string email, string password)
        {
            if (await _context.Users.AnyAsync(u => u.Email == email))
            {
                ModelState.AddModelError("Email", "Email already registered.");
                return View();
            }

            var newUser = new User
            {
                Name = name,
                Email = email,
                Password = password
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();
            
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, email),
                new Claim(ClaimTypes.NameIdentifier, newUser.Id.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "TrainingPlans");
        }

        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
        
        public IActionResult AccessDenied() => View();
    }
    
}

