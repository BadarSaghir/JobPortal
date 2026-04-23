using Career635.Features.Jobs;
using Career635.Features.Jobs.Models;
using Microsoft.AspNetCore.Mvc;
using Paramore.Darker;

public class JobsController : Controller
{
    private readonly IQueryProcessor _queryProcessor;
    public JobsController(IQueryProcessor queryProcessor) => _queryProcessor = queryProcessor;

    [HttpGet("Search")]
    public async Task<IActionResult> Search(string? searchTerm="")
    {
        var result = await _queryProcessor.ExecuteAsync(new GetJobSearchQuery { SearchTerm = searchTerm });
        return View(result);
    }

    [HttpGet("Details/{id}")]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _queryProcessor.ExecuteAsync(new GetJobDetailQuery { Id = id });
        if (result == null) return NotFound();
        return View(result);
    }

    [HttpGet("Track")]
public async Task<IActionResult> Track(string? code)
{
    if (string.IsNullOrWhiteSpace(code))
    {
        return View(new TrackViewModel(null, false, null));
    }

    var result = await _queryProcessor.ExecuteAsync(new GetTrackStatusQuery { TrackingCode = code });
    return View(new TrackViewModel(code, true, result));
}
}