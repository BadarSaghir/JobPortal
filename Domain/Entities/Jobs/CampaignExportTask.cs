namespace Career635.Domain.Entities.Jobs;

public class CampaignExportTask : BaseEntity
{
    public Guid CampaignId { get; set; }
    public Guid RequestedByUserId { get; set; }
    public string Status { get; set; } = "Pending"; // Pending, Processing, Completed, Failed
    public string? DownloadUrl { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime? ProcessedAt { get; set; }

    public virtual RecruitmentCampaign? Campaign { get; set; } 
}