using Career635.Domain.Entities;

namespace Career635.Domain.Entities.Applicants;

public class ApplicantDocument : BaseEntity
{
    public Guid? ApplicantId { get; set; }

    public string DocumentType { get; set; } = string.Empty; // e.g. "Matric Certificate", "CNIC Back"
    public string FileUrl { get; set; } = string.Empty;      // Local storage path
    public DateTimeOffset UploadedAt { get; set; } = DateTimeOffset.UtcNow;

    // Navigation
    public virtual Applicant? Applicant { get; set; } = null!;
}