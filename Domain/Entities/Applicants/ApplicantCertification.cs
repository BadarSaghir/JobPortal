namespace Career635.Domain.Entities.Applicants;

public class ApplicantCertification : BaseEntity
{
    public Guid ApplicantId { get; set; }
    public string CertificateName { get; set; } = string.Empty;
    public string IssuingBody { get; set; } = string.Empty;
    public DateTime? ExpiryDate { get; set; }
            public virtual required Applicant? Applicant { get; set; }

}