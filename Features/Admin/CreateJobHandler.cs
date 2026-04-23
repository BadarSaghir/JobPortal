
using Career635.Domain.Entities.Jobs;
using Paramore.Brighter;
using Career635.Infrastructure.Persistence;

public class CreateJobCommand(JobOpening job, string? skills) : Command(new Id(Guid.NewGuid().ToString()))
{
    public JobOpening Job { get; } = job;
    public string? Skills { get; } = skills;
}

public class CreateJobHandler(AppDbContext context) : RequestHandlerAsync<CreateJobCommand>
{
    public override async Task<CreateJobCommand> HandleAsync(CreateJobCommand command, CancellationToken ct = default)
    {
        // 1. Automatic Slug Generation if empty
        if (string.IsNullOrWhiteSpace(command.Job.JobSlug))
        {
            command.Job.JobSlug = command.Job.Title.ToLower().Replace(" ", "-") + "-" + DateTime.UtcNow.Ticks.ToString().Substring(10);
        }

        // 2. Process Comma-Separated Skills
        if (!string.IsNullOrWhiteSpace(command.Skills))
        {
            var names = command.Skills.Split(',').Select(s => s.Trim()).Where(s => !string.IsNullOrEmpty(s));
            foreach (var name in names)
            {
                command.Job.RequiredSkills.Add(new JobSkillRequirement { SkillName = name });
            }
        }

        context.JobOpenings.Add(command.Job);
        await context.SaveChangesAsync(ct);

        return await base.HandleAsync(command, ct);
    }
}