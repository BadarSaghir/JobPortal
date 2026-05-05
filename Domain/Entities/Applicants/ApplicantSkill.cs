namespace Career635.Domain.Entities.Applicants;

public class ApplicantSkill : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public string SkillName { get; set; } = string.Empty;
    public string Proficiency { get; set; } = string.Empty; // Expert, Intermediate, Beginner
                public virtual  Applicant? Applicant { get; set; }

}