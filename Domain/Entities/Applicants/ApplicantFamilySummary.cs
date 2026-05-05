namespace Career635.Domain.Entities.Applicants;

public class ApplicantFamilySummary : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public int BrothersTotal { get; set; }
    public int BrothersMarried { get; set; }
    public int BrothersUnmarried { get; set; }
    public int SistersTotal { get; set; }
    public int SistersMarried { get; set; }
    public int SistersUnmarried { get; set; }
    public int ChildrenTotal { get; set; }
    public int ChildrenMarried { get; set; }
    public int ChildrenUnmarried { get; set; }
            public virtual  Applicant? Applicant { get; set; }

}