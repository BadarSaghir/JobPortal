using Career635.Areas.Admin.Models;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

public class GetAdminDashboardQuery : IQuery<DashboardViewModel> 
{ 
    public string? SearchTerm { get; set; }
    public string? StatusFilter { get; set; } // NEW
    public int PageNumber { get; set; } = 1;
}

public class GetAdminDashboardHandler(AppDbContext context) : QueryHandlerAsync<GetAdminDashboardQuery, DashboardViewModel>
{
    private const int PageSize = 10;

    public override async Task<DashboardViewModel> ExecuteAsync(GetAdminDashboardQuery query, CancellationToken ct = default)
    {
        // 1. Summary Stats
        var totalJobs = await context.JobOpenings.CountAsync(ct);
        var totalApps = await context.JobApplications.CountAsync(ct);
        var pending = await context.JobApplications.CountAsync(x => x.Status == "Pending", ct);
        var shortlisted = await context.JobApplications.CountAsync(x => x.Status == "Shortlisted", ct);

        // 2. Build Searchable/Filterable Query
        var appQuery = context.JobApplications
            .AsNoTracking()
            .Include(a => a.Applicant)
            .Include(a => a.JobOpening)
            .AsQueryable();

        // Filter by Search Term
        if (!string.IsNullOrWhiteSpace(query.SearchTerm))
        {
            var term = query.SearchTerm.ToLower();
            appQuery = appQuery.Where(a => 
                a.Applicant.FullName.ToLower().Contains(term) || 
                a.Applicant.TrackingCode.ToLower().Contains(term) ||
                a.JobOpening.Title.ToLower().Contains(term));
        }

        // Filter by Status
        if (!string.IsNullOrWhiteSpace(query.StatusFilter))
        {
            appQuery = appQuery.Where(a => a.Status == query.StatusFilter);
        }

        // 3. Handle Pagination
        var totalFiltered = await appQuery.CountAsync(ct);
        var totalPages = (int)Math.Ceiling(totalFiltered / (double)PageSize);
        
        var items = await appQuery
            .OrderByDescending(a => a.AppliedAt)
            .Skip((query.PageNumber - 1) * PageSize)
            .Take(PageSize)
            .Select(a => new RecentApplicationDto(
                a.Id,
                a.Applicant.FullName,
                a.JobOpening.Title,
                a.Applicant.TrackingCode,
                a.Status,
                a.AppliedAt
            ))
            .ToListAsync(ct);

        return new DashboardViewModel(
            totalJobs, totalApps, pending, shortlisted, items, 
            query.PageNumber, totalPages, query.SearchTerm) 
            { StatusFilter = query.StatusFilter }; // Ensure this exists in ViewModel
    }
}