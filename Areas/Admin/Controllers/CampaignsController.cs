using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Career635.Areas.Admin.Models;
using Career635.Features.Admin;
using Career635.Infrastructure.Persistence;
using Career635.Infrastructure.Jobs;
using Career635.Domain.Entities.Jobs;
using Paramore.Brighter;
using Paramore.Darker;
using Quartz;
using Career635.Domain.Constants;

namespace Career635.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Policy = AppPermissions.CampaignsManage)] // Entire controller restricted
[Route("[area]/[controller]")]
public class CampaignsController(
    IQueryProcessor queryProcessor,
    IAmACommandProcessor commandProcessor,
    ISchedulerFactory schedulerFactory,
    AppDbContext context,
    IConfiguration config,
    IWebHostEnvironment env) : Controller
{
    // 1. LISTING WITH PAGINATION
[HttpGet]
    public async Task<IActionResult> Index(string? searchTerm, bool? statusFilter, int page = 1)
    {
        var query = new GetPagedCampaignsQuery 
        { 
            SearchTerm = searchTerm, 
            StatusFilter = statusFilter, 
            Page = page 
        };
        var result = await queryProcessor.ExecuteAsync(query);
        return View(result);
    }

    // 2. CREATE NEW CAMPAIGN
    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCampaignViewModel model)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid campaign data provided.";
            return RedirectToAction(nameof(Index));
        }

        var campaign = new RecruitmentCampaign
        {
            Name = model.Name,
            CampaignCode = model.CampaignCode.ToUpper(),
            IsActive = model.IsActive
        };

        // Execute via Brighter Command
        await commandProcessor.SendAsync(new CreateCampaignCommand(campaign));

        TempData["Success"] = $"Campaign '{model.Name}' has been initiated.";
        return RedirectToAction(nameof(Index));
    }
[HttpPost("UpdateName")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> UpdateName(Guid id, string name)
{
    if (string.IsNullOrWhiteSpace(name)) return BadRequest();
    await commandProcessor.SendAsync(new UpdateCampaignCommand(id, name));
    return RedirectToAction(nameof(Index));
}
    // 3. REQUEST BACKGROUND ZIP EXPORT (QUARTZ)
    [HttpPost("RequestExport/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestExport(Guid id)
    {
        var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(adminIdClaim, out var adminId)) return Unauthorized();

        // Check if this campaign exists and isn't already processing
        var existingTask = await context.Set<CampaignExportTask>()
            .FirstOrDefaultAsync(t => t.CampaignId == id);

        if (existingTask != null && existingTask.Status == "Processing")
        {
            TempData["Info"] = "A dossier export is currently being generated. Please wait.";
            return RedirectToAction(nameof(Index));
        }

        // Initialize or Update Task Record (1 Zip per Campaign Policy)
        if (existingTask == null)
        {
            existingTask = new CampaignExportTask
            {
                CampaignId = id,
                RequestedByUserId = adminId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow
            };
            context.Add(existingTask);
        }
        else
        {
            existingTask.Status = "Pending";
            existingTask.ErrorMessage = null;
            existingTask.ProcessedAt = null;
            existingTask.RequestedByUserId = adminId;
        }

        await context.SaveChangesAsync();

        // Queue the Quartz Job
        var scheduler = await 
        schedulerFactory.GetScheduler();
        var job = JobBuilder.Create<CampaignZipJob>()
            .WithIdentity($"ZipJob-{id}", "CampaignExports")
            .UsingJobData("TaskId", existingTask.Id.ToString())
            .Build();

        var trigger = TriggerBuilder.Create()
            .WithIdentity($"Trigger-{id}", "CampaignExports")
            .StartNow()
            .Build();

        await scheduler.ScheduleJob(job, trigger);

        TempData["Success"] = "The export task has been queued. Large dossiers will take a few minutes to process.";
        return RedirectToAction(nameof(Index));
    }

    // 4. PHYSICAL FILE DOWNLOAD
    [HttpGet("Download/{id}")]
    public async Task<IActionResult> Download(Guid id)
    {
        var task = await context.Set<CampaignExportTask>()
            .Include(t => t.Campaign)
            .FirstOrDefaultAsync(t => t.CampaignId == id && t.Status == "Completed");

        if (task == null || string.IsNullOrEmpty(task.DownloadUrl))
        {
            TempData["Error"] = "Download file not found or export not completed.";
            return RedirectToAction(nameof(Index));
        }

        // Resolve physical path from relative DB path
        string root = config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
        string baseDir = root.StartsWith("wwwroot") ? Path.Combine(env.ContentRootPath, root) : root;
        
        // DownloadUrl looks like: /uploads/exports/FILE.zip
        var fileName = Path.GetFileName(task.DownloadUrl);
        var filePath = Path.Combine(baseDir, "exports", fileName);

        if (!System.IO.File.Exists(filePath))
        {
            TempData["Error"] = "The physical ZIP file has been removed from the server.";
            return RedirectToAction(nameof(Index));
        }

        var fileBytes = await System.IO.File.ReadAllBytesAsync(filePath);
        return File(fileBytes, "application/zip", fileName);
    }

    // 5. TOGGLE CAMPAIGN STATUS
    [HttpPost("ToggleStatus/{id}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ToggleStatus(Guid id)
    {
        var campaign = await context.RecruitmentCampaigns.FindAsync(id);
        if (campaign == null) return NotFound();

        campaign.IsActive = !campaign.IsActive;
        await context.SaveChangesAsync();

        TempData["Success"] = $"Campaign status updated to {(campaign.IsActive ? "Active" : "Paused")}.";
        return RedirectToAction(nameof(Index));
    }
}