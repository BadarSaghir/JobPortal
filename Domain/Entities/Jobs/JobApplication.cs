using Career635.Domain.Entities.Applicants;

namespace Career635.Domain.Entities.Jobs;

public class JobApplication : BaseEntity
{
    public Guid JobOpeningId { get; set; }
    public Guid ApplicantId { get; set; }

    public string Status { get; set; } = "Pending"; // Current Status
    public decimal MatchScore { get; set; }
    public string? RecruiterRemarks { get; set; }
    public DateTimeOffset AppliedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation
    public virtual JobOpening JobOpening { get; set; } = null!;
    public virtual Applicant Applicant { get; set; } = null!;

    // NEW: Audit trail of all status changes
    public virtual ICollection<ApplicationStatusHistory> StatusHistory { get; set; } = new List<ApplicationStatusHistory>();
}