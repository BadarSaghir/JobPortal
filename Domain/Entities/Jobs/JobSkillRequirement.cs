using Career635.Domain.Common;

namespace Career635.Domain.Entities.Jobs;

public class JobSkillRequirement : BaseEntity
{
    public Guid JobOpeningId { get; set; }
    
    public string SkillName { get; set; } = string.Empty;
    
    // Allows recruiters to mark if a skill is absolutely necessary
    public bool IsMandatory { get; set; } 

    // Navigation back to Job
    public virtual JobOpening JobOpening { get; set; } = null!;
}