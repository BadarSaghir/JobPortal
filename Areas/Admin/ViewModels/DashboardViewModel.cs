namespace Career635.Areas.Admin.Models;

public record DashboardViewModel(
    int TotalActiveJobs,
    int TotalApplications,
    int PendingReviews,
    int ShortlistedCount,
    IEnumerable<RecentApplicationDto> RecentApplications,
    int PageNumber,
    int TotalPages,
    string? SearchTerm
) {
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;

    public string? StatusFilter;
}

public record RecentApplicationDto(
    Guid ApplicationId,
    string ApplicantName,
    string JobTitle,
    string TrackingCode,
    string Status,
    DateTimeOffset AppliedAt
);