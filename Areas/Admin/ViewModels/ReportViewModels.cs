namespace Career635.Areas.Admin.Models;

public record ReportDashboardViewModel(
    int TotalApplications,
    int ShortlistedCount,
    int RejectedCount,
    IEnumerable<EducationBreakdownDto> EducationStats,
    IEnumerable<JobPerformanceDto> TopJobs,
    IEnumerable<DailyTrendDto> ApplicationTrend
);

public record EducationBreakdownDto(string Level, int Count, double Percentage);
public record JobPerformanceDto(string Title, int ApplicationCount, double AvgMatchScore);
public record DailyTrendDto(string Date, int Count);