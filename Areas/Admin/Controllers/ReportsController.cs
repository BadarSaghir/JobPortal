using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Paramore.Darker;

[Area("Admin")]
[Authorize(Roles = "SuperAdmin")]
[Route("[area]/[controller]")]
public class ReportsController(IQueryProcessor queryProcessor) : Controller
{
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var result = await queryProcessor.ExecuteAsync(new GetRecruitmentReportQuery());
        return View(result);
    }
}