using Career635.Domain.Entities;

namespace Career635.Domain.Entities.Jobs;

public class RecruitmentCampaign : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string CampaignCode { get; set; } = string.Empty; 
    public bool IsActive { get; set; } = true;

    // Navigation
    public virtual ICollection<JobOpening> JobOpenings { get; set; } = new List<JobOpening>();
}