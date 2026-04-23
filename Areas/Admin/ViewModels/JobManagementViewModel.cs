using System.ComponentModel.DataAnnotations;

namespace Career635.Areas.Admin.Models;

public class CreateJobViewModel
{
    [Required, MaxLength(256)]
    public string Title { get; set; } = string.Empty;

    public string? JobCategory { get; set; } // Technical, Admin, etc.
    public string? EmploymentType { get; set; } // Permanent, Contract
    public int? TotalPositions { get; set; } = 1;
    public string? SalaryGrade { get; set; }

    // --- MARKDOWN FIELDS ---
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public string Requirements { get; set; } = string.Empty;
    public string? Benefits { get; set; }

    // --- CONSTRAINTS & MERIT ---
    public string? LocationType { get; set; } = "On-site";
    public string? WorkLocation { get; set; }
    public int? MinAge { get; set; }
    public int? MaxAge { get; set; }
    
    [Required]
    public decimal RequiredExperienceYears { get; set; }=0;
    [Required]
    public string MinEducationLevel { get; set; } = string.Empty;
    public string? RequiredMajorField { get; set; }
    public bool IsPecRequired { get; set; }

    // --- SYSTEM & SEO ---
    public Guid? CampaignId { get; set; }
    public bool IsFeatured { get; set; }
    public string? JobSlug { get; set; }
        public string? Status { get; set; } = "Published"; 

    [Required]
    public DateTime ExpiresAt { get; set; } = DateTime.Now.AddDays(30);
     [Required]
    public DateTime PostedAt { get; set; } = DateTime.Now;

    // Skills handling (comma separated)
    public string? RequiredSkillsRaw { get; set; }
}