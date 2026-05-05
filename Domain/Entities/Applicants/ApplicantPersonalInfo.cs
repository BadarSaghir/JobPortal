using Career635.Domain.Entities.Locations;

namespace Career635.Domain.Entities.Applicants;

public class ApplicantPersonalInfo : BaseEntity
{
    public Guid? ApplicantId { get; set; }
    public string CandidateType { get; set; } = string.Empty;
    public string FatherName { get; set; } = string.Empty;
    public string? FatherCNIC { get; set; }
    public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty; 

    public string MaritalStatus { get; set; } = string.Empty;
    public string Religion { get; set; } = string.Empty;
    public string? Caste { get; set; }
    public string? Sect { get; set; }
    public string ContactNo { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? PECNumber { get; set; } // For Engineering Council
        public string? Accommodation { get; set; }=string.Empty; // For Engineering Council

    public string PresentAddress { get; set; }=string.Empty;
            public virtual Applicant? Applicant { get; set; }

    public string  PermanentAddress { get; set; }= string.Empty;
}