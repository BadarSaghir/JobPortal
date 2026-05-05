namespace Career635.Domain.Entities.Applicants;

public class ApplicantFinancialDetail : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public decimal? CurrentSalary { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public string? OtherBenefits { get; set; }
    public string? OtherFacilities { get; set; } // Page 6 Select Option
    public string? FamilyIncomeDetail { get; set; } // Page 6 Select Option
                public virtual  Applicant? Applicant { get; set; }

}