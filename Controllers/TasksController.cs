using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;
using Work_Track.Data;
using Work_Track.DTOs;
using Work_Track.Models;

namespace Work_Track.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/tasks
        [HttpPost]
        public IActionResult CreateTask(TaskCreateDto dto)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var task = new TaskItem
            {
                Title = dto.Title,
                Priority = dto.Priority ?? "MED",
                UserId = userId,
                Description = dto.Description,
                DueDate = dto.DueDate
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            if (dto.DueDate < DateTime.UtcNow.Date)
            {
                return BadRequest("Due date cannot be in the past");
            }


            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        // GET: api/tasks
        [HttpGet]
        public IActionResult GetMyTasks()
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var tasks = _context.Tasks
                .Where(t => t.UserId == userId)
                .ToList();

            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound("Task not found");

           return Ok(task);
        }

        // PUT: api/tasks/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = _context.Tasks
                .FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound("Task not found");

            task.Title = dto.Title;
            task.Priority = dto.Priority ?? task.Priority;
            task.Description = dto.Description;
            task.DueDate = dto.DueDate;

            _context.SaveChanges();

           return Ok(task);
        }

        // PUT: api/tasks/{id}/complete
        [HttpPut("{id}/complete")]
        public IActionResult CompleteTask(int id)
        {
            var userId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!
            );

            var task = _context.Tasks
                .FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound("Task not found");

            if (task.IsCompleted)
                return BadRequest("Task is already completed");

            task.IsCompleted = true;
            task.CompletedAt = DateTime.UtcNow;

            _context.SaveChanges();

            return Ok(new
            {
                message = "Task marked as completed",
                taskId = task.Id,
                completedAt = task.CompletedAt
            });
        }

        //undo a completed task
        [HttpPut("{id}/toggle")]
        public IActionResult ToggleTask(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = _context.Tasks
                .FirstOrDefault(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound();

            task.IsCompleted = !task.IsCompleted;
            task.CompletedAt = task.IsCompleted ? DateTime.UtcNow : null;

            _context.SaveChanges();

            return Ok(task);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var task = await _context.Tasks
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (task == null)
                return NotFound("Task not found");

            _context.Tasks.Remove(task);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Task deleted successfully" });
        }
    }
}