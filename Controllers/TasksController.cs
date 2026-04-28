using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    // Helper to get logged-in user id from JWT
    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    // Create Task
    [HttpPost]
    public async Task<IActionResult> CreateTask(CreateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = GetUserId();

        // Check if project belongs to user
        var project = _context.Projects
            .FirstOrDefault(p => p.Id == request.ProjectId && p.UserId == userId);

        if (project == null)
            return NotFound("Project not found");

        var task = new TaskItem
        {
            Title = request.Title,
            Description = request.Description,
            Status = "Todo",
            ProjectId = request.ProjectId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return Ok(task);
    }

    // Get Tasks by Project
    [HttpGet]
    public async Task<IActionResult> GetTasks([FromQuery] TaskQueryParams query)
    {
         if (query.ProjectId <= 0)
    {
        return BadRequest("ProjectId is required");
    }
        var userId = GetUserId();

        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == query.ProjectId && p.UserId == userId);

        if (project == null)
            return NotFound("Project not found");

        var tasksQuery = _context.Tasks
            .Where(t => t.ProjectId == query.ProjectId)
            .AsQueryable();

        // Filter
        if (!string.IsNullOrEmpty(query.Status))
        {
            tasksQuery = tasksQuery.Where(t => t.Status == query.Status);
        }

        tasksQuery = tasksQuery.OrderBy(t => t.Id);

        // Total count
        var totalCount = await tasksQuery.CountAsync();

        // Pagination
        var tasks = await tasksQuery
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return Ok(new
        {
            totalCount,
            query.Page,
            query.PageSize,
            data = tasks
        });
    }

    // Update Task
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTask(int id, UpdateTaskRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound();

        task.Title = request.Title;
        task.Description = request.Description;
        task.Status = request.Status;

        await _context.SaveChangesAsync();

        return Ok(task);
    }

    // Delete Task
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTask(int id)
    {
        var task = _context.Tasks.FirstOrDefault(t => t.Id == id);

        if (task == null)
            return NotFound();

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return Ok("Task deleted");
    }
}