using Career635.Domain.Entities;
using Career635.Domain.Entities.Jobs; // Added for the relationship link

namespace Career635.Domain.Entities.Applicants;

public class Applicant : BaseEntity
{
    // --- 1. CORE IDENTIFICATION (PAGE 1) ---
    public string FullName { get; set; } = string.Empty;
    
    /// <summary>
    /// Unique National Identity. Crucial for preventing duplicate 8-page entries.
    /// </summary>
    public string CNICNumber { get; set; } = string.Empty;
    
    // --- 2. PRIMARY ASSETS (PAGE 2) ---
    /// <summary>
    /// Required Passport Size Photo path
    /// </summary>
    public string? PassportImageUrl { get; set; } 
    
    /// <summary>
    /// Required CV/Resume file path
    /// </summary>
    public string? CvUrl { get; set; }

    // --- 3. SYSTEM TRACKING ---
    /// <summary>
    /// Professional code (e.g. C635-X9Z) for public status tracking
    /// </summary>
public string TrackingCode { get; set; } = string.Empty;
    
    public DateTime AppliedAt { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Audit field to track the last time the 8-page profile was updated
    /// </summary>
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // --- 4. 1:1 RELATIONSHIPS (DETAILED BIO-DATA SLICES) ---
    // Using = null! to satisfy the non-nullable warning while EF handles initialization.
    public virtual ApplicantPersonalInfo PersonalInfo { get; set; } = null!;
    public virtual ApplicantFamilySummary FamilySummary { get; set; } = null!;
    public virtual ApplicantFinancialDetail FinancialDetail { get; set; } = null!;
    public virtual ApplicantMilitaryDetail MilitaryDetail { get; set; } = null!;

    // --- 5. 1:N RELATIONSHIPS (HISTORICAL & REPEATER DATA) ---
    public virtual ICollection<ApplicantEducation> Educations { get; set; } = new List<ApplicantEducation>();
    public virtual ICollection<ApplicantExperience> Experiences { get; set; } = new List<ApplicantExperience>();
    public virtual ICollection<ApplicantSibling> Siblings { get; set; } = new List<ApplicantSibling>();
    public virtual ICollection<ApplicantInternalRelative> InternalRelatives { get; set; } = new List<ApplicantInternalRelative>();
    public virtual ICollection<ApplicantCertification> Certifications { get; set; } = new List<ApplicantCertification>();
    public virtual ICollection<ApplicantSkill> Skills { get; set; } = new List<ApplicantSkill>();
    public virtual ICollection<ApplicantAchievement> Achievements { get; set; } = new List<ApplicantAchievement>();
    public virtual ICollection<ApplicantDocument> Documents { get; set; } = new List<ApplicantDocument>();

   
}