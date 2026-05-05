using Career635.Features.Jobs;
using Career635.Features.Jobs.Models;
using Microsoft.AspNetCore.Mvc;
using Paramore.Darker;

public class JobsController : Controller
{
    private readonly IQueryProcessor _queryProcessor;
    private readonly IWebHostEnvironment _env;
    private readonly IConfiguration _config;
    public JobsController(IQueryProcessor queryProcessor,IConfiguration config, IWebHostEnvironment env){ 
        _queryProcessor = queryProcessor;
        _config=config;
        _env=env;
    
     
    
    }

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

[HttpGet("GetFile")]
public IActionResult GetFile(string path)
{
    // 1. Resolve Path (Consistent with your FileStorageService)
    string root = _config.GetValue<string>("StorageSettings:DocumentRoot") ?? "uploads";
     string baseDir = root.StartsWith("wwwroot") 
        ? Path.Combine(_env.WebRootPath) // If in wwwroot, use WebRootPath
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