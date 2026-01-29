using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Work_Track.Data;
using System.Security.Claims;

namespace Work_Track.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        // GET: /api/dashboard/stats
        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var totalTasks = _context.Tasks.Count(t => t.UserId == userId);
            var completedTasks = _context.Tasks.Count(t => t.UserId == userId && t.IsCompleted);
            var pendingTasks = _context.Tasks.Count(t => t.UserId == userId && !t.IsCompleted);

            return Ok(new
            {
                totalTasks,
                completedTasks,
                pendingTasks
            });
        }


        //get recent tasks
        [HttpGet("recent-tasks")]
        public IActionResult GetRecentTasks()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var tasks = _context.Tasks
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CompletedAt ?? t.CreatedAt)
                .Take(5)
                .Select(t => new
                {
                    t.Id,
                    t.Title,
                    t.IsCompleted,
                    t.Priority,
                    t.CreatedAt,
                    t.CompletedAt
                })
                .ToList();

            return Ok(tasks);
        }
    }
}