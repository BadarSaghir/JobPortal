// Step 4: Education
using Career635.Domain.Entities.Applicants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApplicantEducationConfiguration : IEntityTypeConfiguration<ApplicantEducation>
{
    public void Configure(EntityTypeBuilder<ApplicantEducation> builder)
    {
        builder.ToTable("ApplicantEducations");
        builder.Property(e => e.Qualification).IsRequired().HasMaxLength(100);
        builder.Property(e => e.BoardUniversity).IsRequired().HasMaxLength(200);

          // Link to Degree Level Master Data
        builder.HasOne(e => e.DegreeLevel)
               .WithMany()
               .HasForeignKey(e => e.DegreeLevelId)
               .OnDelete(DeleteBehavior.Restrict);
        builder.HasOne<Applicant>().WithMany(a => a.Educations).HasForeignKey(e => e.ApplicantId);
    }
}

// Step 4 & 5: Experience
public class ApplicantExperienceConfiguration : IEntityTypeConfiguration<ApplicantExperience>
{
    public void Configure(EntityTypeBuilder<ApplicantExperience> builder)
    {
   builder.ToTable("ApplicantExperiences");

        builder.Property(e => e.OrganizationName).IsRequired().HasMaxLength(250);
        builder.Property(e => e.Designation).IsRequired().HasMaxLength(150);
        
        // Configuration for the new detailed field
        builder.Property(e => e.KeyResponsibilities)
               .HasMaxLength(2000); // Allow up to 2000 characters for details

        builder.HasOne(e => e.Applicant)
               .WithMany(a => a.Experiences)
               .HasForeignKey(e => e.ApplicantId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

// Step 6: Internal Relatives
public class ApplicantInternalRelativeConfiguration : IEntityTypeConfiguration<ApplicantInternalRelative>
{
    public void Configure(EntityTypeBuilder<ApplicantInternalRelative> builder)
    {
        builder.ToTable("ApplicantInternalRelatives");
        builder.Property(r => r.RelativeName).IsRequired().HasMaxLength(200);
        builder.HasOne<Applicant>().WithMany(a => a.InternalRelatives).HasForeignKey(r => r.ApplicantId);
    }
}

// Step 8: Sibling Details
public class ApplicantSiblingConfiguration : IEntityTypeConfiguration<ApplicantSibling>
{
    public void Configure(EntityTypeBuilder<ApplicantSibling> builder)
    {
        builder.ToTable("ApplicantSiblings");
        builder.Property(s => s.Name).IsRequired().HasMaxLength(200);
        builder.Property(s => s.CNIC).HasMaxLength(15);
        builder.HasOne<Applicant>().WithMany(a => a.Siblings).HasForeignKey(s => s.ApplicantId);
    }
}