using System.ComponentModel.DataAnnotations;

namespace Career635.Domain.Entities.Jobs;

public class JobOpening : BaseEntity
{
    public Guid? CampaignId { get; set; }

    // --- Basic Info ---
    public string Title { get; set; } = string.Empty;
    public string? JobCategory { get; set; }  // NEW: For grouping (Technical/Admin)
    public string? EmploymentType { get; set; } // NEW: (Full-time/Contract)
    public int? TotalPositions { get; set; }   // NEW: How many seats available
    
    // --- Markdown Content ---
    public string Description { get; set; } = string.Empty;   // RENDER AS MARKDOWN
    public string Requirements { get; set; } = string.Empty;  // RENDER AS MARKDOWN
    public string? Benefits { get; set; } = string.Empty;     // NEW: RENDER AS MARKDOWN
    
    // --- Optional Context ---
    public string? LocationType { get; set; } 
    public string? WorkLocation { get; set; } 
    public int? MinAge { get; set; } 
    public int? MaxAge { get; set; }
    public string? SalaryGrade { get; set; } // NEW: e.g. "Equivalent to BPS-18"
    
    // --- Strict Merit Criteria ---
    public decimal RequiredExperienceYears { get; set; }
    public string MinEducationLevel { get; set; } = string.Empty; 
    public string? RequiredMajorField { get; set; } 
    public bool IsPecRequired { get; set; } 
    
    // --- Logic & Meta ---
    public string Status { get; set; } = "Published"; 
    public bool IsFeatured { get; set; } = false; // NEW: Show at top
    public string? JobSlug { get; set; } // NEW: For SEO URLs
    [Required]
    public DateTime PostedAt { get; set; } = DateTime.Now; // Defaults to today

    [Required]
    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddDays(30);
    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();

    public virtual RecruitmentCampaign? Campaign { get; set; }
    public virtual ICollection<JobSkillRequirement> RequiredSkills { get; set; } = new List<JobSkillRequirement>();
}


