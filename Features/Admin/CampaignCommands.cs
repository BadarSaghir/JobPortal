using Career635.Domain.Entities.Jobs;
using Career635.Infrastructure.Persistence;
using Paramore.Brighter;

public class CreateCampaignCommand(RecruitmentCampaign campaign) : Command(new Id(Guid.NewGuid().ToString()))
{
    public RecruitmentCampaign Campaign { get; } = campaign;
}

public class CreateCampaignHandler(AppDbContext context) : RequestHandlerAsync<CreateCampaignCommand>
{
    public override async Task<CreateCampaignCommand> HandleAsync(CreateCampaignCommand command, CancellationToken ct = default)
    {
        context.RecruitmentCampaigns.Add(command.Campaign);
        await context.SaveChangesAsync(ct);
        return await base.HandleAsync(command, ct);
    }
}