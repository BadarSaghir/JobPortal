using System.Security.Claims;
using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Auth;
using Career635.Domain.Entities.Jobs;
using Career635.Infrastructure.Persistence;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;
using Paramore.Darker;

[Area("Admin")]
[Authorize]
[Microsoft.AspNetCore.Components.Route("[area]/[ontroller]")]
public class AccountAdminController(IQueryProcessor queryProcessor,AppDbContext _context, IAmACommandProcessor commandProcessor,UserManager<ApplicationUser> userManager) : Controller
{
    [HttpGet("Profile")]
    public async Task<IActionResult> Profile()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var result = await queryProcessor.ExecuteAsync(new GetUserProfileQuery { UserId = userId });
        return View(result);
    }
    
    [HttpGet("Settings")]
    public IActionResult Settings() => View();

[HttpPost("ChangePassword")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
{
    if (!ModelState.IsValid) return View("Settings", model);

    var user = await userManager.GetUserAsync(User);
    if (user == null) return NotFound();

    var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
    
    if (result.Succeeded)
    {
        TempData["Success"] = "Security credentials updated successfully.";
        return RedirectToAction(nameof(Settings));
    }

    foreach (var error in result.Errors)
    {
        ModelState.AddModelError(string.Empty, error.Description);
    }

    return View("Settings");
}

[HttpGet("Edit/{id}")]
public async Task<IActionResult> Edit(Guid id)
{
    var job = await _context.JobOpenings.Include(j => j.RequiredSkills).FirstOrDefaultAsync(j => j.Id == id);
    if (job == null) return NotFound();

    var model = job.Adapt<EditJobViewModel>();
    model.RequiredSkillsRaw = string.Join(", ", job.RequiredSkills.Select(s => s.SkillName));

    ViewBag.Campaigns = await _context.RecruitmentCampaigns.Where(c => !c.IsDeleted).ToListAsync();
    ViewBag.DegreeLevels = await _context.DegreeLevels.OrderBy(x => x.LevelOrder).ToListAsync();

    return View(model);
}

[HttpPost("Edit/{id}")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(EditJobViewModel model)
{
    if (!ModelState.IsValid) return View(model);
    
    var entity = model.Adapt<JobOpening>();
    await commandProcessor.SendAsync(new UpdateJobCommand(entity, model.RequiredSkillsRaw));
    
    return RedirectToAction(nameof(Index));
}
}