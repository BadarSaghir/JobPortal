// --- JOB UPDATE ---
using Career635.Domain.Entities.Jobs;
using Career635.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Paramore.Brighter;



public class UpdateJobCommand(JobOpening job, List<string>? skillNames) : Command(new Id(Guid.NewGuid().ToString())) 
{
    public JobOpening Job { get; } = job;
    public List<string> SkillNames { get; } = skillNames ?? new();
}
public class UpdateJobHandler(AppDbContext context) : RequestHandlerAsync<UpdateJobCommand>
{
    public override async Task<UpdateJobCommand> HandleAsync(UpdateJobCommand command, CancellationToken ct = default)
    {
           var existingJob = await context.JobOpenings
            .Include(j => j.RequiredSkills)
            .FirstOrDefaultAsync(j => j.Id == command.Job.Id, ct);

        if (existingJob == null)
        {
            throw new KeyNotFoundException($"Job ID {command.Job.Id} not found in registry.");
        }

        // 1. Explicitly map only the allowed business properties.
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
         existingJob.PostedAt = DateTime.SpecifyKind(command.Job.PostedAt, DateTimeKind.Local).ToUniversalTime();
        existingJob.ExpiresAt = DateTime.SpecifyKind(command.Job.ExpiresAt, DateTimeKind.Local).ToUniversalTime();
        existingJob.CampaignId = command.Job.CampaignId; // Cleaned up redundant ??null
 if (existingJob.RequiredSkills.Any())
        {
            context.JobSkillRequirements.RemoveRange(existingJob.RequiredSkills);
        }
       if (command.SkillNames != null && command.SkillNames.Any())
        {
            foreach (var name in command.SkillNames.Where(n => !string.IsNullOrWhiteSpace(n)).Distinct())
            {
                var newSkill = new JobSkillRequirement
                {
                    Id = Guid.NewGuid(), // Generate new ID
                    JobOpeningId = existingJob.Id,
                    SkillName = name.Trim(),
                    CreatedAt = DateTimeOffset.UtcNow,
                    IsDeleted = false
                };
                context.JobSkillRequirements.Add(newSkill);
            }
        }

        // 4. Save
        try 
        {
            await context.SaveChangesAsync(ct);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new Exception("Concurrency error: Requisition was modified by another user.", ex);
        }
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