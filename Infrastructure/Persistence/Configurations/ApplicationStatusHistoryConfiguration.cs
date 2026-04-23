using Career635.Domain.Entities.Jobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApplicationStatusHistoryConfiguration : IEntityTypeConfiguration<ApplicationStatusHistory>
{
    public void Configure(EntityTypeBuilder<ApplicationStatusHistory> builder)
    {
        builder.ToTable("ApplicationStatusHistory");

        builder.HasOne(h => h.JobApplication)
               .WithMany(a => a.StatusHistory)
               .HasForeignKey(h => h.JobApplicationId);

        // Link to the staff member who changed the status
        builder.HasOne(h => h.ChangedByUser)
               .WithMany()
               .HasForeignKey(h => h.ChangedByUserId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}