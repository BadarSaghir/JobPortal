using Career635.Domain.Entities.Auth;
using Career635.Infrastructure.Persistence;
using System.Security.Claims;

public class AuditMiddleware(RequestDelegate next)
{
      private static readonly string[] ExcludedExtensions = 
    { 
        ".jpg", ".jpeg", ".png", ".gif", ".svg", ".ico", // Images
        ".zip", ".rar", ".pdf", ".docx", ".xlsx",       // Files
        ".css", ".js", ".map", ".woff", ".woff2"        // Assets
    };
    public async Task InvokeAsync(HttpContext context, AppDbContext dbContext)
    {
        // 1. Capture basic request info
        var path = context.Request.Path.ToString();
        
        // Skip logging for static files and assets
        if (path.Contains(".") || path.Contains("/dist/")) {
            await next(context);
            return;
        }

            bool isStaticFile = ExcludedExtensions.Any(ext => path.EndsWith(ext));

        // Also ignore standard asset folders
        bool isAssetFolder = path.StartsWith("/lib/") || path.StartsWith("/dist/") || path.StartsWith("/uploads/");

        if (isStaticFile || isAssetFolder || path.Contains("/getfile"))
        {
            await next(context);
            return;
        }


        var ip = context.Connection.RemoteIpAddress?.ToString();

            // 2. CAPTURE METADATA
        
        // Handle IP if behind a proxy (Nginx/Docker on Linux)
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            ip = context.Request.Headers["X-Forwarded-For"];
        var userAgent = context.Request.Headers["User-Agent"].ToString();
        var userId = context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var userName = context.User?.Identity?.Name;

        // 2. Log the visit
        var log = new AuditLog
        {
            UserId = userId,
            UserName = userName ?? "Anonymous",
            Action = context.Request.Method,
            Path = path,
            IPAddress = ip,
            UserAgent = userAgent,
                     EntityName = "RouteAccess",
                CreatedAt = DateTimeOffset.UtcNow

        };

        dbContext.Set<AuditLog>().Add(log);
        await dbContext.SaveChangesAsync();

        await next(context);
    }
}