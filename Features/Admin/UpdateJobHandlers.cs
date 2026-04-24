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
        var existingJob = await context.JobOpenings
            .Include(j => j.RequiredSkills).Include(k=>k.Campaign)
            .FirstOrDefaultAsync(j => j.Id == command.Job.Id, ct);

        if (existingJob == null)
        {
            throw new KeyNotFoundException($"Job ID {command.Job.Id} not found in registry.");
        }

        // 2. CRITICAL FIX: Explicitly map only the allowed business properties.
        // Doing this instead of .SetValues() prevents EF Core from overwriting BaseEntity 
        // fields (like RowVersion, IsDeleted) with null, which causes Concurrency Exceptions.
        existingJob.Title = command.Job.Title;
        existingJob.JobCategory = command.Job.JobCategory;
        existingJob.EmploymentType = command.Job.EmploymentType;
        existingJob.TotalPositions = command.Job.TotalPositions;
        existingJob.Description = command.Job.Description;
        existingJob.Requirements = command.Job.Requirements;
        existingJob.Benefits = command.Job.Benefits;
        existingJob.LocationType = command.Job.LocationType;
        existingJob.WorkLocation = command.Job.WorkLocation;
        existingJob.MinAge = command.Job.MinAge;
        existingJob.MaxAge = command.Job.MaxAge;
        existingJob.SalaryGrade = command.Job.SalaryGrade;
        existingJob.RequiredExperienceYears = command.Job.RequiredExperienceYears;
        existingJob.MinEducationLevel = command.Job.MinEducationLevel;
        existingJob.RequiredMajorField = command.Job.RequiredMajorField;
        existingJob.IsPecRequired = command.Job.IsPecRequired;
        existingJob.Status = command.Job.Status;
        existingJob.IsFeatured = command.Job.IsFeatured;
        existingJob.JobSlug = command.Job.JobSlug;
        existingJob.PostedAt = command.Job.PostedAt;
        existingJob.ExpiresAt = command.Job.ExpiresAt;
        existingJob.CampaignId = command.Job.CampaignId??null;

        // 3. Handle Skills (Manual Sync)
      

        if (!string.IsNullOrWhiteSpace(command.Skills))
        {
              if (existingJob.RequiredSkills.Any())
        {
            context.JobSkillRequirements.RemoveRange(existingJob.RequiredSkills);
        }
            var skillNames = command.Skills.Split(',')
                .Select(s => s.Trim())
                .Where(s => !string.IsNullOrEmpty(s));

            foreach (var name in skillNames)
            {
                existingJob.RequiredSkills.Add(new JobSkillRequirement { 
                    SkillName = name,
                    JobOpeningId = existingJob.Id 
                });
            }
        }

        // 4. Save Changes
        await context.SaveChangesAsync(ct);

        return await base.HandleAsync(command, ct);
    }
}
public class UpdateCampaignCommand(Guid CampaignId , string name) : Command(new Id(Guid.NewGuid().ToString())) {
    public Guid CampaignId  { get; } = CampaignId ;
    public string Name { get; } = name;
}

public class UpdateCampaignHandler(AppDbContext context) : RequestHandlerAsync<UpdateCampaignCommand> {
    public override async Task<UpdateCampaignCommand> HandleAsync(UpdateCampaignCommand command, CancellationToken ct = default) {
        var campaign = await context.RecruitmentCampaigns.FindAsync(command.CampaignId );
        if (campaign != null) {
            campaign.Name = command.Name;
            await context.SaveChangesAsync(ct);
        }
        return await base.HandleAsync(command, ct);
    }
}