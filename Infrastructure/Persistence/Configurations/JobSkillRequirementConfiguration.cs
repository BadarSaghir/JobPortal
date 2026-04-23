using Career635.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class JobSkillRequirementConfiguration : IEntityTypeConfiguration<JobSkillRequirement>
{
    public void Configure(EntityTypeBuilder<JobSkillRequirement> builder)
    {
        builder.ToTable("JobSkillRequirements");
        
        builder.Property(s => s.SkillName).IsRequired().HasMaxLength(100);

        builder.HasOne(s => s.JobOpening)
               .WithMany(j => j.RequiredSkills)
               .HasForeignKey(s => s.JobOpeningId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}