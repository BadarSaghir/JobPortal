using Career635.Areas.Admin.Models;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Darker;


public class GetRecruitmentReportQuery : IQuery<ReportDashboardViewModel> { }

public class GetRecruitmentReportHandler(AppDbContext context) : QueryHandlerAsync<GetRecruitmentReportQuery, ReportDashboardViewModel>
{
    public override async Task<ReportDashboardViewModel> ExecuteAsync(GetRecruitmentReportQuery query, CancellationToken ct = default)
    {
        // 1. High-Level KPIs
        var total = await context.JobApplications.CountAsync(ct);
        var shortlisted = await context.JobApplications.CountAsync(x => x.Status == "Shortlisted", ct);
        var rejected = await context.JobApplications.CountAsync(x => x.Status == "Rejected", ct);
        var pending = await context.JobApplications.CountAsync(x => x.Status == "Pending", ct);

        // 2. Education Distribution (Grouping by Degree Level Name)
        var eduData = await context.ApplicantEducations
            .GroupBy(e => e.DegreeLevel.Name)
            .Select(g => new { Level = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .ToListAsync(ct);

        var educationStats = eduData.Select(x => new EducationStatDto(
            x.Level, x.Count, total > 0 ? (double)x.Count / total * 100 : 0
        ));

        // 3. Application Trend (Last 14 Days)
        var twoWeeksAgo = DateTime.UtcNow.Date.AddDays(-14);
        var trendData = await context.JobApplications
            .Where(a => a.AppliedAt >= twoWeeksAgo)
            .GroupBy(a => a.AppliedAt.Date)
            .OrderBy(g => g.Key)
            .Select(g => new DailyTrendDto(g.Key.ToString("MMM dd"), g.Count()))
            .ToListAsync(ct);

        // 4. Job Performance (Top 5 roles by volume)
        var topJobs = await context.JobOpenings
            .AsNoTracking()
            .OrderByDescending(j => j.JobApplications.Count)
            .Take(5)
            .Select(j => new JobPerformanceDto(j.Id, j.Title, j.JobApplications.Count, j.Status))
            .ToListAsync(ct);

        return new ReportDashboardViewModel(total, shortlisted, rejected, pending, educationStats, trendData, topJobs);
    }
}