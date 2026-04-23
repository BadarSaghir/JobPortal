using Career635.Domain.Entities;

namespace Career635.Domain.Entities.Applicants;

public class ApplicantInternalRelative : BaseEntity
{
    public Guid ApplicantId { get; set; }

    public string RelativeName { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string PayScale { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;

    // Navigation
            public virtual  Applicant? Applicant { get; set; }
}