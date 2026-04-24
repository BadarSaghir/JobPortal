using Career635.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Quartz.Xml.JobSchedulingData20;

public class JobOpeningConfiguration : IEntityTypeConfiguration<JobOpening>
{
    public void Configure(EntityTypeBuilder<JobOpening> builder)
    {
        builder.ToTable("JobOpenings");
        builder.HasKey(j => j.Id);

        builder.Property(j => j.Title).IsRequired().HasMaxLength(256);        
        // Decimal Precision for Experience (e.g., 5.5 years)
        builder.Property(j => j.RequiredExperienceYears).HasPrecision(18, 2);

        // Optional Fields
        builder.Property(j => j.LocationType).HasMaxLength(50);
        builder.Property(j => j.WorkLocation).HasMaxLength(150);
        builder.Property(j => j.Status).HasMaxLength(50);

        // Optional Relationship to Campaign
        builder.HasOne(j => j.Campaign)
               .WithMany(c => c.JobOpenings)
               .HasForeignKey(j => j.CampaignId)
               .IsRequired(false)
               .OnDelete(DeleteBehavior.SetNull);
builder.HasMany(j => j.JobApplications)
           .WithOne(a => a.JobOpening)
           .HasForeignKey(a => a.JobOpeningId)
           .OnDelete(DeleteBehavior.Restrict);

        // --- PERFORMANCE INDEXES ---
        builder.HasIndex(j => j.PostedAt);   // For "Recent Jobs"
        builder.HasIndex(j => j.ExpiresAt);  // For "Closing Soon"
        builder.HasIndex(j => j.Status);     // Filter for "Published" only
    }
}