using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;
using Career635.Infrastructure.Persistence;
using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Jobs;
using Mapster;
using Paramore.Darker;
using Career635.Features.Admin;

namespace Career635.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
// This ensures the URL remains "/Admin/Jobs" even though the class is "JobsAdminController"
[Route("[Area]/[Controller]")] 
public class JobsAdminController : Controller
{
    private readonly IQueryProcessor _queryProcessor; // Changed from Context to QueryProcessor
    private readonly IAmACommandProcessor _commandProcessor;
    private readonly AppDbContext _context;

  public JobsAdminController(IQueryProcessor queryProcessor, IAmACommandProcessor commandProcessor, AppDbContext context)
    {
        _queryProcessor = queryProcessor;
        _commandProcessor = commandProcessor;
        _context=context;
    
    }
  [HttpGet]
public async Task<IActionResult> Index(string? searchTerm, string? status, string? category, int pageNumber = 1)
{
    var query = new GetAdminJobsQuery { 
        SearchTerm = searchTerm, 
        Status = status, 
        Category = category, 
        PageNumber = pageNumber 
    };
    var result = await _queryProcessor.ExecuteAsync(query);
    return View(result);
}
[HttpGet("SearchCampaigns")]
public async Task<IActionResult> SearchCampaigns(string q)
{
    // Fetch top 10 matching active/non-deleted campaigns
    var campaigns = await _context.RecruitmentCampaigns
        .AsNoTracking()
        .Where(c => c.IsActive && !c.IsDeleted && 
                   (c.Name.Contains(q) || c.CampaignCode.Contains(q)))
        .OrderBy(c => c.Name)
        .Take(10)
        .Select(c => new { value = c.Id, text = $"{c.Name} ({c.CampaignCode})" })
        .ToListAsync();

    return Json(campaigns);
}
    [HttpGet("Create")] // URL: /Admin/Jobs/Create
    public async Task<IActionResult> Create()
    {
        // Safety check for DB context
        if (_context == null) return Content("Database context is not initialized.");

        ViewBag.Campaigns = await _context.RecruitmentCampaigns
            .Where(c => c.IsActive && !c.IsDeleted)
            .ToListAsync();

        ViewBag.DegreeLevels = await _context.DegreeLevels
            .OrderBy(x => x.LevelOrder)
            .ToListAsync();

        return View(new CreateJobViewModel());
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateJobViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Campaigns = await _context.RecruitmentCampaigns.Where(c => c.IsActive).ToListAsync();
            ViewBag.DegreeLevels = await _context.DegreeLevels.OrderBy(x => x.LevelOrder).ToListAsync();
            return View(model);
        }

        var entity = model.Adapt<JobOpening>();
        await _commandProcessor.SendAsync(new CreateJobCommand(entity, model.RequiredSkillsRaw));

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Edit/{id}")]
public async Task<IActionResult> Edit(Guid id)
{
    var job = await _context.JobOpenings.Include(j => j.Campaign).Include(j => j.RequiredSkills).FirstOrDefaultAsync(j => j.Id == id);
    if (job == null) return NotFound();

    var model = job.Adapt<EditJobViewModel>();
    model.RequiredSkillsRaw = string.Join(", ", job.RequiredSkills.Select(s => s.SkillName));
    ViewBag.CurrentCampaignName = job.Campaign?.Name;

    ViewBag.Campaigns = await _context.RecruitmentCampaigns.Where(c => !c.IsDeleted).ToListAsync();
    ViewBag.DegreeLevels = await _context.DegreeLevels.OrderBy(x => x.LevelOrder).ToListAsync();

    return View(model);
}

[HttpPost("Edit/{id}")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(Guid id, EditJobViewModel model)
{
    // Security Check: ID in URL must match ID in Hidden Field
    if (id != model.Id) return BadRequest();

    if (!ModelState.IsValid)
    {
        // Re-populate dropdowns if returning to view
        ViewBag.DegreeLevels = await _context.DegreeLevels.OrderBy(x => x.LevelOrder).ToListAsync();
        return View(model);
    }

    // Map ViewModel to Entity
    var entity = model.Adapt<JobOpening>();
    
    // ENSURE the ID is explicitly set on the entity before sending to Brighter
    entity.Id = id; 

    await _commandProcessor.SendAsync(new UpdateJobCommand(entity, model.RequiredSkillsRaw));

    return RedirectToAction(nameof(Index));
}
}