using Career635.Areas.Admin.Models;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

public class GetAdminJobsQuery : IQuery<AdminJobIndexViewModel> 
{ 
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public string? Category { get; set; }
    public int PageNumber { get; set; } = 1;
}

public class GetAdminJobsHandler(AppDbContext context) : QueryHandlerAsync<GetAdminJobsQuery, AdminJobIndexViewModel>
{
    private const int PageSize = 10;

    public override async Task<AdminJobIndexViewModel> ExecuteAsync(GetAdminJobsQuery query, CancellationToken ct = default)
    {
        var dbQuery = context.JobOpenings.AsNoTracking().Where(j => !j.IsDeleted);

        // 1. Apply Search
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            dbQuery = dbQuery.Where(j => j.Title.Contains(query.SearchTerm));
        }

        // 2. Apply Filters
        if (!string.IsNullOrWhiteSpace(query.Status))
            dbQuery = dbQuery.Where(j => j.Status == query.Status);

        if (!string.IsNullOrWhiteSpace(query.Category))
            dbQuery = dbQuery.Where(j => j.JobCategory == query.Category);

        // 3. Get metadata for UI
        var totalCount = await dbQuery.CountAsync(ct);
        var categories = await context.JobOpenings.Select(j => j.JobCategory).Distinct().ToListAsync(ct);
        var totalPages = (int)Math.Ceiling(totalCount / (double)PageSize);

        // 4. Fetch Paginated Items
        var items = await dbQuery
            .OrderByDescending(j => j.CreatedAt)
            .Skip((query.PageNumber - 1) * PageSize)
            .Take(PageSize)
            .Select(j => new AdminJobSummaryViewModel(
                j.Id, j.Title, j.JobCategory ?? "General", j.Status, j.PostedAt, j.ExpiresAt,
                context.JobApplications.Count(a => a.JobOpeningId == j.Id),
                j.IsFeatured,
                j.Campaign != null ? j.Campaign.Name : "Independent"
            ))
            .ToListAsync(ct);

        return new AdminJobIndexViewModel(items, query.PageNumber, totalPages, query.SearchTerm, query.Status, query.Category, categories!);
    }
}