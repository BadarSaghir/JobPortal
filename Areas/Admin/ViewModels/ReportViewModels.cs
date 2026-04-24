namespace Career635.Areas.Admin.Models;

public record ReportDashboardViewModel(
    int TotalApplications,
    int ShortlistedCount,
    int RejectedCount,
    int PendingCount,
    IEnumerable<EducationStatDto> EducationStats,
    IEnumerable<DailyTrendDto> AcquisitionTrend,
    IEnumerable<JobPerformanceDto> TopPerformers
);

public record EducationStatDto(string Level, int Count, double Percentage);
public record DailyTrendDto(string DateLabel, int Count);
public record JobPerformanceDto(Guid JobId, string Title, int Count, string Status);