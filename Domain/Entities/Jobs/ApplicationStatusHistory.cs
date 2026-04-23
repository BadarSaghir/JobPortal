using Career635.Domain.Entities.Auth;

namespace Career635.Domain.Entities.Jobs;

public class ApplicationStatusHistory : BaseEntity
{
    public Guid JobApplicationId { get; set; }
    
    public string Status { get; set; } = string.Empty; // The new status (e.g., "Interviewing")
    public string? Remarks { get; set; }               // Why was it changed?
    
    // Internal Staff member who performed the action
    public Guid ChangedByUserId { get; set; } 

    // Navigations
    public virtual JobApplication JobApplication { get; set; } = null!;
    public virtual ApplicationUser ChangedByUser { get; set; } = null!;
}