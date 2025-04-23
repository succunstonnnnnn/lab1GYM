using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GymInfrastructure;

namespace GymInfrastructure.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly GYMDbContext _context;

        public ChartsController(GYMDbContext context)
        {
            _context = context;
        }

        [HttpGet("trainingDescriptions")]
        public async Task<JsonResult> GetTrainingDescriptionsAsync()
        {
            var result = await _context.TrainingPlans
                .GroupBy(tp => tp.Description)
                .Select(g => new { Description = g.Key ?? "No description", Count = g.Count() })
                .ToListAsync();

            return new JsonResult(result);
        }

        [HttpGet("nutritionPlans")]
        public async Task<JsonResult> GetNutritionPlansAsync()
        {
            var result = await _context.NutritionPlans
                .GroupBy(np => np.Name)
                .Select(g => new { Name = g.Key, Count = g.Count() })
                .ToListAsync();

            return new JsonResult(result);
        }
    }
}

