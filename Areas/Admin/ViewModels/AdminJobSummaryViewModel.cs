namespace Career635.Areas.Admin.Models;

// public record AdminJobSummaryViewModel(
//     Guid Id,
//     string Title,
//     string JobCategory,
//     string Status,
//     DateTime PostedAt,
//     DateTime ExpiresAt,
//     int ApplicantCount,
//     bool IsFeatured,
//     string? CampaignName
// );



public record AdminJobIndexViewModel(
    IEnumerable<AdminJobSummaryViewModel> Items,
    int PageNumber,
    int TotalPages,
    string? SearchTerm,
    string? StatusFilter,
    string? CategoryFilter,
    IEnumerable<string> AvailableCategories
) {
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public record AdminJobSummaryViewModel(
    Guid Id,
    string Title,
    string JobCategory,
    string Status,
    DateTime PostedAt,
    DateTime ExpiresAt,
    int ApplicantCount,
    bool IsFeatured,
    string? CampaignName
);