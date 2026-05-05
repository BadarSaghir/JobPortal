namespace Career635.Domain.Entities.Applicants;

public class ApplicantMilitaryDetail : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public string? ArmyNumber { get; set; }
    public string? ArmyUnit { get; set; }
    public string? ArmyCharacter { get; set; }
    public string? ArmyPayScale { get; set; }
                public virtual  Applicant? Applicant { get; set; }

}