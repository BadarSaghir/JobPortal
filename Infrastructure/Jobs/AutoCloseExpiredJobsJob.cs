using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Career635.Infrastructure.Jobs;

[DisallowConcurrentExecution] // Ensure only one instance runs at a time
public class AutoCloseExpiredJobsJob(
    AppDbContext context, 
    ILogger<AutoCloseExpiredJobsJob> logger) : IJob
{
    public async Task Execute(IJobExecutionContext contextJob)
    {
        logger.LogInformation("Background Task: Scanning for expired personnel requisitions...");

        // We use local time as per your requirement for the home page logic
        var now = DateTime.Now;

        try
        {
            // Update all Published jobs that have passed their expiry date
            // This is a high-performance batch update
            int affectedRows = await context.JobOpenings
                .Where(j => j.Status == "Published" && j.ExpiresAt < now)
                .ExecuteUpdateAsync(setters => setters.SetProperty(j => j.Status, "Closed"));

            if (affectedRows > 0)
            {
                logger.LogInformation("Successfully archived {Count} expired vacancies.", affectedRows);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An error occurred during the automated job closing sequence.");
        }
    }
}