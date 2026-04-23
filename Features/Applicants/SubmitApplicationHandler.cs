using Paramore.Brighter;
using Career635.Infrastructure.Persistence;
using Career635.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;

namespace Career635.Features.Applicants;

public class SubmitApplicationHandler(AppDbContext context) : RequestHandlerAsync<SubmitApplicationCommand>
{
    public override async Task<SubmitApplicationCommand> HandleAsync(SubmitApplicationCommand command, CancellationToken ct = default)
    {
        // 1. Define the Execution Strategy
        var strategy = context.Database.CreateExecutionStrategy();

        // 2. Wrap all database logic inside ExecuteAsync
        return await strategy.ExecuteAsync(async () =>
        {
            // 3. Start the transaction inside the strategy
            using var transaction = await context.Database.BeginTransactionAsync(ct);

            try
            {
                // BUSINESS RULE: Check for duplicate application
                var alreadyApplied = await context.JobApplications
                    .AnyAsync(ja => ja.JobOpeningId == command.JobId 
                                 && ja.Applicant.CNICNumber == command.ApplicantData.CNICNumber, ct);

                if (alreadyApplied)
                {
                    throw new InvalidOperationException("Our records show you have already submitted an application for this specific position.");
                }

                // Prepare the Applicant Snapshot
                var snapshot = command.ApplicantData;
                string trackingCode = $"C635-{Guid.NewGuid().ToString().ToUpper()}";
                snapshot.TrackingCode = trackingCode;
                command.GeneratedTrackingCode = trackingCode;
                context.Applicants.Add(snapshot);

                // Prepare the Job Application link
                var jobApp = new JobApplication
                {
                    JobOpeningId = command.JobId,
                    Applicant = snapshot, // Linking objects directly so EF handles the IDs
                    AppliedAt = DateTime.UtcNow,
                    Status = "Pending",
                    MatchScore = 0 
                };

                // Add both to context
                context.JobApplications.Add(jobApp);

                // Save changes (Atomic operation)
                await context.SaveChangesAsync(ct);

                // Commit the transaction
                await transaction.CommitAsync(ct);

                // Call the next handler in the Brighter pipeline
                return await base.HandleAsync(command, ct);
            }
            catch (Exception)
            {
                // Explicitly rollback on failure
                await transaction.RollbackAsync(ct);
                throw;
            }
        });
    }
}