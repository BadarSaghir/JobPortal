using Career635.Domain.Common;
using Career635.Domain.Entities.Auth;

namespace Career635.Domain.Entities.Applicants;

public class ApplicantEducation : BaseEntity
{
    public Guid ApplicantId { get; set; }
    
    // Link to Master Data ID
    public Guid? DegreeLevelId { get; set; }

    public string Qualification { get; set; } = string.Empty; // e.g., BS Computer Science
    public string MajorField { get; set; } = string.Empty;    // e.g., Software Engineering
    public string BoardUniversity { get; set; } = string.Empty;
    public string CgpaPercentage { get; set; } = string.Empty;

    // MISSING DATES ADDED
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }

    // Navigations
            public virtual  Applicant? Applicant { get; set; }
    public virtual DegreeLevel? DegreeLevel { get; set; } 
}