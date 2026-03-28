using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }
    private int GetUserId()
    {
        return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject(CreateProjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var project = new Project
        {
            Name = request.Name,
            Description = request.Description,
            UserId = GetUserId()
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return Ok(project);
    }

    [HttpGet]
    public IActionResult GetMyProjects(int page = 1, int pageSize = 5)
    {
        var userId = GetUserId();

        var query = _context.Projects
            .Where(p => p.UserId == userId);

        var totalCount = query.Count();

        var projects = query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        return Ok(new
        {
            totalCount,
            page,
            pageSize,
            data = projects
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, UpdateProjectRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var userId = GetUserId();

        var project = _context.Projects
            .FirstOrDefault(p => p.Id == id && p.UserId == userId);

        if (project == null)
            return NotFound("Project not found");

        project.Name = request.Name;
        project.Description = request.Description;

        await _context.SaveChangesAsync();

        return Ok(project);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var userId = GetUserId();

        var project = _context.Projects
            .FirstOrDefault(p => p.Id == id && p.UserId == userId);

        if (project == null)
            return NotFound("Project not found");

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();

        return Ok("Project deleted");
    }
}