using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Career635.Features.Admin;
using Paramore.Darker;

namespace Career635.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
public class DashboardController(IQueryProcessor queryProcessor) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index(string? searchTerm, int pageNumber = 1)
    {
        var query = new GetAdminDashboardQuery { 
            SearchTerm = searchTerm, 
            PageNumber = pageNumber < 1 ? 1 : pageNumber 
        };
        
        var result = await queryProcessor.ExecuteAsync(query);
        return View(result);
    }
}