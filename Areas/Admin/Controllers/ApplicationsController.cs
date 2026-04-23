using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Paramore.Darker;
using Paramore.Brighter;
using Career635.Features.Admin;
using System.Security.Claims;

namespace Career635.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")] // This dynamically uses "Admin/Applications"
[Authorize(Roles = "SuperAdmin")]
public class ApplicationsController(IQueryProcessor queryProcessor, IAmACommandProcessor commandProcessor,    IConfiguration config, IWebHostEnvironment env) : Controller
{
    // 1. LIST ALL SUBMISSIONS
    [HttpGet]
public async Task<IActionResult> Index(string? searchTerm, string? statusFilter, int pageNumber = 1)
{
    var query = new GetAdminDashboardQuery 
    { 
        SearchTerm = searchTerm, 
        StatusFilter = statusFilter,
        PageNumber = pageNumber < 1 ? 1 : pageNumber 
    };
    
    var result = await queryProcessor.ExecuteAsync(query);
    return View(result); // Return the WHOLE viewmodel
}
    // 2. VIEW FULL DOSSIER
    [HttpGet("Review/{id}")]
    public async Task<IActionResult> Review(Guid id)
    {
        var result = await queryProcessor.ExecuteAsync(new GetApplicationReviewQuery { Id = id });
        if (result == null) return NotFound();
        
        return View(result);
    }

[HttpGet("GetFile")]
public IActionResult GetFile(string path)
{
    // 1. Resolve Path (Consistent with your FileStorageService)
    string root = config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
     string baseDir = root.StartsWith("wwwroot") 
        ? Path.Combine(env.WebRootPath) // If in wwwroot, use WebRootPath
        : Path.GetFullPath(root);   
    string fullPath = Path.GetFullPath(Path.Combine(baseDir, path));

    if (!System.IO.File.Exists(fullPath)) return NotFound();

    // 2. Determine Content Type
    var extension = Path.GetExtension(fullPath).ToLower();
    string contentType = extension switch {
        ".pdf" => "application/pdf",
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        _ => "application/octet-stream"
    };

    return PhysicalFile(fullPath, contentType);
}
    // 3. UPDATE DOSSIER STATUS
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(Guid applicationId, string status, string? remarks)
    {
        // Get the current logged-in Admin's ID from Claims
        var adminIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!Guid.TryParse(adminIdClaim, out var adminId)) return Unauthorized();

        try 
        {
            var command = new UpdateApplicationStatusCommand(applicationId, status, remarks, adminId);
            await commandProcessor.SendAsync(command);
            
            TempData["SuccessMessage"] = "Application status has been updated successfully.";
        }
        catch (Exception)
        {
            TempData["ErrorMessage"] = "Failed to update application status.";
        }

        return RedirectToAction(nameof(Review), new { id = applicationId });
    }
}