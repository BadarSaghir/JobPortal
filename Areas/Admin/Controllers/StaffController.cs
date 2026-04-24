using Career635.Areas.Admin.Models;
using Career635.Domain.Constants;
using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

[Area("Admin")]

[Route("[area]/[controller]")]
[Authorize(Policy = AppPermissions.RolesManage)]

public class StaffController(
    IQueryProcessor queryProcessor, 
    UserManager<ApplicationUser> userManager,
    AppDbContext context) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm, Guid? departmentId, int page = 1)
    {
        ViewBag.Departments = await context.Departments.ToListAsync();
        var result = await queryProcessor.ExecuteAsync(new GetPagedStaffQuery { 
            SearchTerm = searchTerm, DepartmentId = departmentId, Page = page 
        });
        return View(result);
    }

    [HttpGet("Create")]
    public async Task<IActionResult> Create()
    {
        ViewBag.Departments = await context.Departments.ToListAsync();
        ViewBag.Designations = await context.Designations.ToListAsync();
        ViewBag.PayScales = await context.PayScales.ToListAsync();
        ViewBag.Roles = await context.Roles.Select(r => r.Name).ToListAsync();
        return View(new CreateStaffViewModel());
    }

    [HttpPost("Create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStaffViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = new ApplicationUser {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName,
            DepartmentId = model.DepartmentId,
            DesignationId = model.DesignationId,
            PayScaleId = model.PayScaleId,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(user, model.RoleName);
            return RedirectToAction(nameof(Index));
        }

        foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
        return View(model);
    }
}