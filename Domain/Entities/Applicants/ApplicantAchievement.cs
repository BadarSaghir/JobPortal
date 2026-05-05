namespace Career635.Domain.Entities.Applicants;

public class ApplicantAchievement : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime DateReceived { get; set; }
            public virtual  Applicant? Applicant { get; set; }

}