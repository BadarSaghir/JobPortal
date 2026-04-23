using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Career635.Domain.Entities.Jobs;

namespace Career635.Infrastructure.Persistence.Configurations;

public class RecruitmentCampaignConfiguration : IEntityTypeConfiguration<RecruitmentCampaign>
{
    public void Configure(EntityTypeBuilder<RecruitmentCampaign> builder)
    {
        builder.ToTable("RecruitmentCampaigns");
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        
        builder.Property(c => c.CampaignCode)
               .IsRequired()
               .HasMaxLength(50);
        
        builder.HasIndex(c => c.CampaignCode).IsUnique();
    }
}