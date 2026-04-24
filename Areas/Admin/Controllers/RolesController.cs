using Career635.Areas.Admin.Models;
using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;
using Paramore.Darker;

[Area("Admin")]
[Route("[area]/[controller]")]
[Authorize(Roles = "SuperAdmin")]
public class RolesController(
    IQueryProcessor queryProcessor, 
    IAmACommandProcessor commandProcessor,
    AppDbContext context) : Controller
{

    [HttpGet] // URL: /Admin/Roles?page=1
public async Task<IActionResult> Index(int page = 1)
{
    var result = await queryProcessor.ExecuteAsync(new GetPagedRolesQuery { Page = page });
    return View(result);
}
    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        // Fetch all permissions grouped by Module (e.g., Jobs, Applicants)
        var permissions = await context.Permissions
            .Where(p => !p.IsDeleted)
            .GroupBy(p => p.Module)
            .Select(g => new PermissionGroupDto(g.Key, g.Select(p => new PermissionItemDto(p.Id, p.DisplayName, p.Name))))
            .ToListAsync();

        ViewBag.PermissionGroups = permissions;
        return View(new CreateRoleViewModel());
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRoleViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var role = new ApplicationRole(model.Name) { Description = model.Description };
        
        try {
            await commandProcessor.SendAsync(new CreateRoleCommand(role, model.SelectedPermissionIds));
            return RedirectToAction("Index", "Dashboard"); // Redirect to registry
        }
        catch (Exception ex) {
            ModelState.AddModelError("", ex.Message);
            return View(model);
        }
    }



[HttpGet("Edit/{id}")]
public async Task<IActionResult> Edit(Guid id)
{
    var role = await context.Roles
        .Include(r => r.RolePermissions)
        .FirstOrDefaultAsync(r => r.Id == id);

    if (role == null) return NotFound();

    // Prepare ViewModel
    var model = new EditRoleViewModel
    {
        Id = role.Id,
        Name = role.Name!,
        Description = role.Description,
        SelectedPermissionIds = role.RolePermissions.Select(rp => rp.PermissionId).ToList()
    };

    // Load all permissions for the checkbox matrix
    ViewBag.PermissionGroups = await context.Permissions
        .Where(p => !p.IsDeleted)
        .GroupBy(p => p.Module)
        .Select(g => new PermissionGroupDto(g.Key, g.Select(p => new PermissionItemDto(p.Id, p.DisplayName, p.Name))))
        .ToListAsync();

    return View(model);
}

[HttpPost("Edit/{id}")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Edit(Guid id, EditRoleViewModel model)
{
    if (id != model.Id) return BadRequest();

    if (!ModelState.IsValid)
    {
        // Re-fetch groups if returning to view
        return View(model); 
    }

    await commandProcessor.SendAsync(new UpdateRoleCommand(
        model.Id, model.Name, model.Description, model.SelectedPermissionIds));

    return RedirectToAction(nameof(Index));
}
}