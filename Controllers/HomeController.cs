using Microsoft.AspNetCore.Mvc;
using Paramore.Darker;
using Career635.Features.Jobs;

namespace Career635.Controllers;

public class HomeController : Controller
{
    private readonly IQueryProcessor _queryProcessor;

    public HomeController(IQueryProcessor queryProcessor) => _queryProcessor = queryProcessor;

    public async Task<IActionResult> Index()
    {
        var viewModel = await _queryProcessor.ExecuteAsync(new GetHomeJobsQuery());
        return View(viewModel);
    }
}