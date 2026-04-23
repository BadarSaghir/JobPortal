namespace Career635.Domain.Entities.Applicants;

public class ApplicantSibling : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? CNIC { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Occupation { get; set; }
    public string? Organization { get; set; }
    public string? Gender { get; set; }
    public string? MaritalStatus { get; set; }
    public string? Designation { get; set; }


                public virtual  Applicant? Applicant { get; set; }

}