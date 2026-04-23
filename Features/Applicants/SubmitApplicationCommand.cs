using Paramore.Brighter;
using Career635.Domain.Entities.Applicants;

namespace Career635.Features.Applicants;

public class SubmitApplicationCommand : Command
{
    public Applicant ApplicantData  { get; }
    public Guid JobId { get; }
    public string GeneratedTrackingCode { get; set; } = string.Empty;

    public SubmitApplicationCommand(Applicant applicant, Guid jobId) : base(new Id(Guid.NewGuid().ToString()))
    {
        ApplicantData  = applicant;
        JobId = jobId;
    }
}