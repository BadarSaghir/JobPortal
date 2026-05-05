namespace Career635.Features.Jobs.Models;

// Single result for the Search Page
// public record JobSearchResultViewModel(
//     Guid Id,
//     string Title,
//     string? WorkLocation,
//     string? LocationType,
//     string MinEducationLevel,
//     decimal RequiredExperienceYears,
//     DateTime PostedAt
// );

// Full details for the Specification Page
public record JobDetailViewModel(
    Guid Id,
    string Title,
    string? JobCategory,
    string? EmploymentType,
    int? TotalPositions,
    string Description,     // Markdown
    string Requirements,    // Markdown
    string? Benefits,       // Markdown
    string? WorkLocation,
    string? LocationType,
    int? MinAge,
    int? MaxAge,
    string? SalaryGrade,
    string MinEducationLevel,
    string? RequiredMajorField,
    bool IsPecRequired,
    decimal RequiredExperienceYears,
    DateTime PostedAt,
    DateTime ExpiresAt,
    bool IsExpired,
    List<string> RequiredSkills
);

// public record SearchPageViewModel(
//     IEnumerable<JobSearchResultViewModel> Results,
//     string? SearchTerm,
//     int TotalCount
// );