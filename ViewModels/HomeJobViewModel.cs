namespace Career635.Features.Jobs.Models;

public record HomeViewModel(
    IEnumerable<HomeJobViewModel> RecentJobs,
    IEnumerable<HomeJobViewModel> ExpiringJobs,
    IEnumerable<string> JobCategories,    // e.g. Technical, Administrative
    int NewJobsCount,                    // Stats for the badge
    int TotalActiveJobs                  // Stats for the hero
);

public record HomeJobViewModel(
    Guid Id,
    string Title,
    string? WorkLocation,
    string? LocationType,                // Remote/On-site
    string MinEducationLevel,            // From DB
    decimal RequiredExperienceYears,     // From DB
    DateTime PostedAt,
    DateTime ExpiresAt
);