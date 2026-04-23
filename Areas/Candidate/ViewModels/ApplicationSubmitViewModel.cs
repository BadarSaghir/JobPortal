using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Career635.Infrastructure.Attributes; // Location of your MaxFileSize & AllowedExtensions attributes

namespace Career635.Areas.Candidate.Models;

public class ApplicationSubmitViewModel
{
    public Guid JobId { get; set; }

    // --- SECTION 1: CORE IDENTITY ---
    [Required(ErrorMessage = "Full Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot exceed 200 characters")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "CNIC is required")]
    [RegularExpression(@"^\d{5}-\d{7}-\d{1}$", ErrorMessage = "CNIC must be in 00000-0000000-0 format")]
    [StringLength(15)]
    public string CNICNumber { get; set; } = string.Empty;

    // --- SECTION 2: PERSONAL BIO-DATA ---
    [Required]
    [StringLength(200)]
    public string FatherName { get; set; } = string.Empty;
    
    [StringLength(15)]
    public string? FatherCNIC { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-20);
    
    [Required]
    [StringLength(50)]
    public string MaritalStatus { get; set; } = "Single";
    
    [Required]
    [StringLength(50)]
    public string Religion { get; set; } = "Islam";
    [Required]
    public string Gender { get; set; } = "Male"; // Default value for the dropdown
    [StringLength(100)] public string? Caste { get; set; }
    [StringLength(100)] public string? Sect { get; set; }
    [StringLength(100)] public string? Accommodation { get; set; }

    
    [Required]
    [Phone]
    [StringLength(20)]
    public string ContactNo { get; set; } = string.Empty;
    
    [EmailAddress]
    [StringLength(256)]
    public string? Email { get; set; }
    
    [StringLength(50)]
    public string? PECNumber { get; set; } 
    
    [StringLength(50)]
    public string? CandidateType { get; set; } = "Candidate";

    [Required]
    [StringLength(1000)]
    public string PresentAddress { get; set; } = string.Empty;
    
    [Required]
    [StringLength(1000)]
    public string PermanentAddress { get; set; } = string.Empty;

    // --- SECTION 3: FAMILY SUMMARY (COUNTS) ---
    [Range(0, 50)] public int BrothersTotal { get; set; }
    [Range(0, 50)] public int BrothersMarried { get; set; }
    [Range(0, 50)] public int BrothersUnmarried { get; set; }
    [Range(0, 50)] public int SistersTotal { get; set; }
    [Range(0, 50)] public int SistersMarried { get; set; }
    [Range(0, 50)] public int SistersUnmarried { get; set; }
    [Range(0, 50)] public int ChildrenTotal { get; set; }
    [Range(0, 50)] public int ChildrenMarried { get; set; }
    [Range(0, 50)] public int ChildrenUnmarried { get; set; }

    // --- SECTION 4: MILITARY DETAIL ---
    [StringLength(50)] public string? ArmyNumber { get; set; }
    [StringLength(100)] public string? ArmyUnit { get; set; }
    [StringLength(100)] public string? ArmyCharacter { get; set; }
    [StringLength(50)] public string? ArmyPayScale { get; set; }

    // --- SECTION 5: FINANCIALS ---
    [Range(0, 99999999)] public decimal? CurrentSalary { get; set; }
    [Range(0, 99999999)] public decimal? ExpectedSalary { get; set; }
    [StringLength(500)] public string? OtherBenefits { get; set; }
        [StringLength(500)] public string? OtherFacilities { get; set; }

    [StringLength(100)] public string? FamilyIncomeDetail { get; set; }

    // --- SECTION 6: DYNAMIC LISTS (ANTI-INFINITE POSTING LIMITS) ---
    [MaxLength(5, ErrorMessage = "Max 5 Educations allowed")]
    public List<EducationEntry> Educations { get; set; } = new();

    [MaxLength(5, ErrorMessage = "Max 5 Experiences allowed")]
    public List<ExperienceEntry> Experiences { get; set; } = new();

    [MaxLength(50)]
    public List<InternalRelativeEntry> InternalRelatives { get; set; } = new();

    [MaxLength(50)]
    public List<SiblingEntry> Siblings { get; set; } = new();
    
    [MaxLength(50)] // Allow slightly more skills
    public List<SkillEntry> Skills { get; set; } = new();

    [MaxLength(5)]
    public List<CertificationEntry> Certifications { get; set; } = new();

    [MaxLength(10)]
    public List<AchievementEntry> Achievements { get; set; } = new();
public List<DocumentUploadEntry> DocumentAttachments { get; set; } = new();

    // --- SECTION 7: SECURE UPLOADS ---
    [Required(ErrorMessage = "Passport Photo is mandatory")]
    [MaxFileSize(500 * 1024)] // 500 KB limit
    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
    public IFormFile PassportPhoto { get; set; } = null!;

    [Required(ErrorMessage = "CV / Resume is mandatory")]
    [MaxFileSize(2 * 1024 * 1024)] // 2 MB limit
    [AllowedExtensions(new[] { ".pdf", ".doc", ".docx" })]
    public IFormFile CvFile { get; set; } = null!;
}

// --- NESTED DATA MODELS (WITH DB STRING LIMITS) ---

public class EducationEntry {
    public Guid? DegreeLevelId { get; set; }
    [StringLength(100)] public string Qualification { get; set; } = string.Empty;
    [StringLength(200)] public string MajorField { get; set; } = string.Empty;
    [StringLength(200)] public string BoardUniversity { get; set; } = string.Empty;
    [StringLength(20)] public string CgpaPercentage { get; set; } = string.Empty;
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
}

public class ExperienceEntry {
    [StringLength(250)] public string OrganizationName { get; set; } = string.Empty;
    [StringLength(150)] public string Designation { get; set; } = string.Empty;
    [StringLength(2000)] public string? KeyResponsibilities { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime? ToDate { get; set; }
}

public class InternalRelativeEntry {
    [StringLength(200)] public string RelativeName { get; set; } = string.Empty;
    [StringLength(100)] public string Designation { get; set; } = string.Empty;
    [StringLength(100)] public string Department { get; set; } = string.Empty;
    [StringLength(50)] public string PayScale { get; set; } = string.Empty;
}

public class SiblingEntry {
    [StringLength(200)] public string Name { get; set; } = string.Empty;
    [StringLength(15)] public string? CNIC { get; set; }
    public DateTime DateOfBirth { get; set; }
    [StringLength(150)] public string? Occupation { get; set; }
    [StringLength(150)] public string? Gender { get; set; }
    [StringLength(150)] public string? MaritalStatus { get; set; }
    [StringLength(250)] public string? Organization { get; set; }
    [StringLength(250)] public string? Designation { get; set; }
}

public class SkillEntry {
    [StringLength(100)] public string SkillName { get; set; } = string.Empty;
    [StringLength(50)] public string Proficiency { get; set; } = string.Empty;
}

public class CertificationEntry {
    [StringLength(200)] public string CertificateName { get; set; } = string.Empty;
    [StringLength(200)] public string IssuingBody { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
}

public class AchievementEntry {
    [StringLength(200)] public string Title { get; set; } = string.Empty;
    [StringLength(2000)] public string? Description { get; set; }
    public DateTime DateReceived { get; set; }
}

public class DocumentUploadEntry
{
    public string DocumentType { get; set; } = string.Empty; // From Dropdown
    public List<IFormFile> Files { get; set; } = new();
}
