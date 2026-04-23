namespace Career635.Features.Jobs.Models;

public record JobSearchResultViewModel(
    Guid Id,
    string Title,
    string? JobCategory,
    string? WorkLocation,
    string? LocationType,
    string MinEducationLevel,
    decimal RequiredExperienceYears,
    DateTime PostedAt,
    DateTime ExpiresAt
);

public record SearchPageViewModel(
    IEnumerable<JobSearchResultViewModel> Results,
    string? SearchTerm,
    int TotalCount,
    int PageNumber,
    int TotalPages
) {
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}