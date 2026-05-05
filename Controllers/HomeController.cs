using Microsoft.AspNetCore.Mvc;
using Paramore.Darker;
using Career635.Features.Jobs;
using Career635.Features.Support;
using Paramore.Brighter;

namespace Career635.Controllers;

public class HomeController : Controller
{
    private readonly IQueryProcessor _queryProcessor;
   private readonly  IAmACommandProcessor _commandProcessor;

    public HomeController(IQueryProcessor queryProcessor,IAmACommandProcessor commandProcessor){ _queryProcessor = queryProcessor;
    _commandProcessor=commandProcessor;
    }
    public async Task<IActionResult> Index()
    {
        var viewModel = await _queryProcessor.ExecuteAsync(new GetHomeJobsQuery());
        return View(viewModel);
    }

    [HttpPost("SubmitSupport")]
[ValidateAntiForgeryToken]
public async Task<IActionResult> SubmitSupport(string email, string message)
{
    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
        return Json(new { success = false, message = "Email and message are required." });

    var command = new SubmitSupportCommand(
        email, 
        message, 
        Request.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown",
        Request.Headers["User-Agent"].ToString()
    );

    await _commandProcessor.SendAsync(command);

    return Json(new { success = true, message = "Your help ticket has been logged. Priority response queued." });
}
}