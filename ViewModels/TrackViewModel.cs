namespace Career635.Features.Jobs.Models;

public record TrackViewModel(
    string? TrackingCode,
    bool Searched,
    ApplicationStatusDto? Result
);

public record ApplicationStatusDto(
    string JobTitle,
    DateTimeOffset AppliedAt,
    string Status, // Pending, Shortlisted, Interviewed, Rejected, Offered
    string? Remarks
);