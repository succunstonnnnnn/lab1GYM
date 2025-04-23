using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GymInfrastructure.Models;

namespace GymInfrastructure.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult GoToTrainingPlan()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "TrainingPlans");

            return RedirectToAction("Login", "Account", new { returnUrl = "/TrainingPlans" });
        }

        public IActionResult GoToNutritionPlan()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "NutritionPlans");

            return RedirectToAction("Login", "Account", new { returnUrl = "/NutritionPlans" });
        }

        public IActionResult GoToProgress()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "ProgressTrackings");

            return RedirectToAction("Login", "Account", new { returnUrl = "/ProgressTrackings" });
        }
    }
}