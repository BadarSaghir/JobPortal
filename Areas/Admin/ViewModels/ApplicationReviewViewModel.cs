using Career635.Domain.Entities.Applicants;

namespace Career635.Areas.Admin.Models;

public record ApplicationReviewViewModel(
    Guid ApplicationId,
    string Status,
    string? RecruiterRemarks,
    Applicant Applicant, // Contains the full nested tree
    string JobTitle,
    DateTimeOffset AppliedAt
);