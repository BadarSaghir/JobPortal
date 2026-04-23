namespace Career635.Domain.Entities.Applicants;

public class ApplicantExperience : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;

    public string? KeyResponsibilities { get; set; } 
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
            public virtual Applicant? Applicant { get; set; }

}