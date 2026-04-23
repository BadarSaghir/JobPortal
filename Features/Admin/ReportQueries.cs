using Career635.Areas.Admin.Models;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;

public class GetRecruitmentReportQuery : IQuery<ReportDashboardViewModel> { }

public class GetRecruitmentReportHandler(AppDbContext context) : QueryHandlerAsync<GetRecruitmentReportQuery, ReportDashboardViewModel>
{
    public override async Task<ReportDashboardViewModel> ExecuteAsync(GetRecruitmentReportQuery query, CancellationToken ct = default)
    {
        // 1. Core Counts
        var total = await context.JobApplications.CountAsync(ct);
        var shortlisted = await context.JobApplications.CountAsync(x => x.Status == "Shortlisted", ct);
        var rejected = await context.JobApplications.CountAsync(x => x.Status == "Rejected", ct);

        // 2. Education Breakdown (Dynamic Grouping)
        var eduData = await context.ApplicantEducations
            .GroupBy(e => e.Qualification)
            .Select(g => new { Level = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync(ct);

        var educationStats = eduData.Select(x => new EducationBreakdownDto(
            x.Level, x.Count, total > 0 ? (double)x.Count / total * 100 : 0
        ));

        // 3. Job Performance
     var topJobs = await context.JobOpenings
    .AsNoTracking()
    .OrderByDescending(j => j.JobApplications.Count) // Now this works!
    .Take(5)
    .Select(j => new JobPerformanceDto(
        j.Title, 
        j.JobApplications.Count, 
        0 // Placeholder for AvgMatchScore
    ))
    .ToListAsync(ct);

        // 4. Application Trend (Last 7 Days)
        var startDate = DateTime.UtcNow.Date.AddDays(-7);
        var trend = await context.JobApplications
            .Where(a => a.AppliedAt >= startDate)
            .GroupBy(a => a.AppliedAt.Date)
            .Select(g => new DailyTrendDto(g.Key.ToString("MMM dd"), g.Count()))
            .ToListAsync(ct);

        return new ReportDashboardViewModel(total, shortlisted, rejected, educationStats, topJobs, trend);
    }
}