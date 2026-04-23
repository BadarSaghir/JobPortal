// --- JOB UPDATE ---
using Career635.Domain.Entities.Jobs;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;

public class UpdateJobCommand(JobOpening job, string? skills) : Command(new Id(Guid.NewGuid().ToString())) {
    public JobOpening Job { get; } = job;
    public string? Skills { get; } = skills;
}

public class UpdateJobHandler(AppDbContext context) : RequestHandlerAsync<UpdateJobCommand>
{
    public override async Task<UpdateJobCommand> HandleAsync(UpdateJobCommand command, CancellationToken ct = default)
    {
        // 1. Fetch the tracked entity
        var existingJob = await context.JobOpenings
            .Include(j => j.RequiredSkills)
            .FirstOrDefaultAsync(j => j.Id == command.Job.Id, ct);

        if (existingJob == null) throw new KeyNotFoundException("Job not found.");

        // 2. Update scalar properties (handles CampaignId correctly now)
        context.Entry(existingJob).CurrentValues.SetValues(command.Job);
        
        // 3. Prevent overwriting CreatedAt
        context.Entry(existingJob).Property(x => x.CreatedAt).IsModified = false;

        // 4. Handle Skills
        context.JobSkillRequirements.RemoveRange(existingJob.RequiredSkills);
        if (!string.IsNullOrWhiteSpace(command.Skills))
        {
            var names = command.Skills.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s));
            foreach (var name in names)
            {
                existingJob.RequiredSkills.Add(new JobSkillRequirement { SkillName = name });
            }
        }

        await context.SaveChangesAsync(ct);
        return await base.HandleAsync(command, ct);
    }
}

// --- CAMPAIGN UPDATE ---
public class UpdateCampaignCommand(Guid id, string name) : Command(new Id(Guid.NewGuid().ToString())) {
    public Guid Id { get; } = id;
    public string Name { get; } = name;
}

public class UpdateCampaignHandler(AppDbContext context) : RequestHandlerAsync<UpdateCampaignCommand> {
    public override async Task<UpdateCampaignCommand> HandleAsync(UpdateCampaignCommand command, CancellationToken ct = default) {
        var campaign = await context.RecruitmentCampaigns.FindAsync(command.Id);
        if (campaign != null) {
            campaign.Name = command.Name;
            await context.SaveChangesAsync(ct);
        }
        return await base.HandleAsync(command, ct);
    }
}