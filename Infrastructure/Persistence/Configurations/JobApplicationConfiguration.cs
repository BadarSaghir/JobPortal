using Career635.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class JobApplicationConfiguration : IEntityTypeConfiguration<JobApplication>
{
    public void Configure(EntityTypeBuilder<JobApplication> builder)
    {
        builder.ToTable("JobApplications");
        builder.HasKey(ja => ja.Id);

        // Precision for the automated matching score (0.00 to 100.00)
        builder.Property(ja => ja.MatchScore).HasPrecision(5, 2);

        builder.HasOne(ja => ja.JobOpening)
               .WithMany()
               .HasForeignKey(ja => ja.JobOpeningId)
               .OnDelete(DeleteBehavior.NoAction); // Keep app if job is deleted

        builder.HasOne(ja => ja.Applicant)
               .WithMany()
               .HasForeignKey(ja => ja.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction); // Delete app if applicant is removed
    }
}