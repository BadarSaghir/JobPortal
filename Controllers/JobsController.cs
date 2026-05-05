using Career635.Features.Jobs;
using Career635.Features.Jobs.Models;
using Microsoft.AspNetCore.Mvc;
using Paramore.Darker;

public class JobsController : Controller
{
    private readonly IQueryProcessor _queryProcessor;
    public JobsController(IQueryProcessor queryProcessor) => _queryProcessor = queryProcessor;

[HttpGet("Search")]
public async Task<IActionResult> Search(string? searchTerm = "", int pageNumber = 1)
{
    // Ensure pageNumber is at least 1
    var currentPage = pageNumber < 1 ? 1 : pageNumber;

    var query = new GetJobSearchQuery 
    { 
        SearchTerm = searchTerm, 
        PageNumber = currentPage 
    };

    var result = await _queryProcessor.ExecuteAsync(query);
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
public async Task<IActionResult> Track(string? code, int pageNumber = 1)
{
    if (string.IsNullOrWhiteSpace(code))
    {
        return View(new TrackViewModel(null, false, null));
    }

    var query = new GetTrackStatusQuery { 
        TrackingCode = code, 
        PageNumber = pageNumber < 1 ? 1 : pageNumber 
    };

    var result = await _queryProcessor.ExecuteAsync(query);
    return View(new TrackViewModel(code, true, result));
}
}