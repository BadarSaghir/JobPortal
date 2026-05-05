using System.ComponentModel.DataAnnotations;

namespace Career635.Areas.Admin.Models;

public record CampaignSummaryViewModel(
    Guid Id,
    string Name,
    string CampaignCode,
    bool IsActive,
    int JobCount,
    DateTime CreatedAt,
    string ExportStatus, 
    string? DownloadUrl
);

public record CampaignIndexViewModel(
    IEnumerable<CampaignSummaryViewModel> Items,
    int PageNumber,
    int TotalPages,
    string? SearchTerm,
    bool? StatusFilter, // null = All, true = Active, false = Paused
    int TotalCount
) {
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}

public class CreateCampaignViewModel
{
    [Required, MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string CampaignCode { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    
}





