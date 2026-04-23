using Paramore.Darker;
using Microsoft.EntityFrameworkCore;
using Career635.Infrastructure.Persistence;
using Career635.Features.Jobs.Models;

namespace Career635.Features.Jobs;

public class GetHomeJobsQuery : IQuery<HomeViewModel> { }

public class GetHomeJobsHandler(AppDbContext context) : QueryHandlerAsync<GetHomeJobsQuery, HomeViewModel>
{
    public override async Task<HomeViewModel> ExecuteAsync(GetHomeJobsQuery query, CancellationToken ct = default)
    {
        // Use local time as requested for expiration logic
        var now = DateTime.Now; 
        var weekAgo = now.AddDays(-7);
        var expiringThreshold = now.AddHours(48);

        // 1. Fetch Recent Jobs 
        // Filter: Must be Published AND PostedAt has passed AND hasn't expired yet
        var recent = await context.JobOpenings
            .AsNoTracking()
            .Where(j => j.Status == "Published" 
                     && j.PostedAt <= now 
                     && j.ExpiresAt >= now)
            .OrderByDescending(j => j.PostedAt)
            .Take(6)
            .Select(j => new HomeJobViewModel(
                j.Id, j.Title, j.WorkLocation, j.LocationType, 
                j.MinEducationLevel, j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt))
            .ToListAsync(ct);

        // 2. Fetch Expiring Jobs (Urgent)
        // Filter: Published AND PostedAt passed AND expires within next 48 hours
        var expiring = await context.JobOpenings
            .AsNoTracking()
            .Where(j => j.Status == "Published" 
                     && j.PostedAt <= now
                     && j.ExpiresAt <= expiringThreshold 
                     && j.ExpiresAt > now)
            .OrderBy(j => j.ExpiresAt)
            .Select(j => new HomeJobViewModel(
                j.Id, j.Title, j.WorkLocation, j.LocationType, 
                j.MinEducationLevel, j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt))
            .ToListAsync(ct);

        // 3. Dynamic Categories from currently active jobs only
        var categories = await context.JobOpenings
            .Where(j => j.Status == "Published" && j.PostedAt <= now && j.ExpiresAt >= now)
            .Select(j => j.MinEducationLevel)
            .Distinct()
            .ToListAsync(ct);

        // 4. Global Stats (Active vs New)
        var totalActive = await context.JobOpenings.CountAsync(j => j.Status == "Published" && j.PostedAt <= now && j.ExpiresAt >= now, ct);
        var newThisWeek = await context.JobOpenings.CountAsync(j => j.Status == "Published" && j.PostedAt <= now && j.PostedAt >= weekAgo && j.ExpiresAt > now, ct);

        return new HomeViewModel(recent, expiring, categories, newThisWeek, totalActive);
    }
}

// --- SEARCH QUERY ---
public class GetJobSearchQuery : IQuery<SearchPageViewModel> {
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 9; // Grid of 3x3
}

public class GetJobSearchHandler(AppDbContext context) : QueryHandlerAsync<GetJobSearchQuery, SearchPageViewModel>
{
    public override async Task<SearchPageViewModel> ExecuteAsync(GetJobSearchQuery query, CancellationToken ct = default)
    {
        var now = DateTime.Now;
        var dbQuery = context.JobOpenings.AsNoTracking()
            .Where(j => j.Status == "Published" && j.PostedAt <= now && j.ExpiresAt > now);

        if (!string.IsNullOrWhiteSpace(query.SearchTerm)) {
            var term = query.SearchTerm.ToLower();
            dbQuery = dbQuery.Where(j => j.Title.ToLower().Contains(term) || j.Description.ToLower().Contains(term));
        }

        var totalCount = await dbQuery.CountAsync(ct);
        
        var results = await dbQuery.OrderByDescending(j => j.PostedAt)
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(j => new JobSearchResultViewModel(
                j.Id, j.Title, j.JobCategory, j.WorkLocation, j.LocationType, 
                j.MinEducationLevel, j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt))
            .ToListAsync(ct);

        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        return new SearchPageViewModel(results, query.SearchTerm, totalCount, query.PageNumber, totalPages);
    }
}
// --- DETAIL QUERY ---
public class GetJobDetailQuery : IQuery<JobDetailViewModel?> {
    public Guid Id { get; set; }
}

public class GetJobDetailHandler : QueryHandlerAsync<GetJobDetailQuery, JobDetailViewModel?>
{
    private readonly AppDbContext _context;
    public GetJobDetailHandler(AppDbContext context) => _context = context;

public override async Task<JobDetailViewModel?> ExecuteAsync(GetJobDetailQuery query, CancellationToken ct = default)
{
    var j = await _context.JobOpenings
        .AsNoTracking()
        .Include(x => x.RequiredSkills) // Ensure skills are loaded
        .FirstOrDefaultAsync(x => x.Id == query.Id, ct);

    if (j == null) return null;

    return new JobDetailViewModel(
        j.Id, j.Title,j.JobCategory,j.EmploymentType,j.TotalPositions, j.Description, j.Requirements,j.Benefits ,j.WorkLocation, j.LocationType, 
        j.MinAge, j.MaxAge,j.SalaryGrade, j.MinEducationLevel, j.RequiredMajorField, j.IsPecRequired, 
        j.RequiredExperienceYears, j.PostedAt, j.ExpiresAt, 
        j.ExpiresAt < DateTime.UtcNow,
        j.RequiredSkills.Select(s => s.SkillName).ToList()
    );
}
}


public class GetTrackStatusQuery : IQuery<ApplicationStatusDto?> 
{
    public string TrackingCode { get; set; } = string.Empty;
}

public class GetTrackStatusHandler : QueryHandlerAsync<GetTrackStatusQuery, ApplicationStatusDto?>
{
    private readonly AppDbContext _context;
    public GetTrackStatusHandler(AppDbContext context) => _context = context;

    public override async Task<ApplicationStatusDto?> ExecuteAsync(GetTrackStatusQuery query, CancellationToken ct = default)
    {
        // Join Application with Applicant to find by TrackingCode
        var app = await _context.JobApplications
            .AsNoTracking()
            .Include(ja => ja.JobOpening)
            .Include(ja => ja.Applicant)
            .FirstOrDefaultAsync(ja => ja.Applicant.TrackingCode == query.TrackingCode, ct);

        if (app == null) return null;

        return new ApplicationStatusDto(
            app.JobOpening.Title,
            app.AppliedAt,
            app.Status,
            app.RecruiterRemarks
        );
    }
}