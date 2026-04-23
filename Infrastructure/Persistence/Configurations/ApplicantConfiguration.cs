using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Career635.Domain.Entities.Applicants;

namespace Career635.Infrastructure.Persistence.Configurations;

public class ApplicantConfiguration : IEntityTypeConfiguration<Applicant>
{
    public void Configure(EntityTypeBuilder<Applicant> builder)
    {
        builder.ToTable("Applicants");
        builder.HasKey(a => a.Id);

        // --- CORE PROPERTIES (PAGE 1 & 2) ---
        builder.Property(a => a.FullName)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(a => a.CNICNumber)
               .IsRequired()
               .HasMaxLength(15);
        
        builder.HasIndex(a => a.CNICNumber); // Prevent duplicate CNICs

        builder.Property(a => a.TrackingCode)
               .IsRequired()
               .HasMaxLength(50);
        
        builder.HasIndex(a => a.TrackingCode).IsUnique(); // Unique for status tracking

        builder.Property(a => a.PassportImageUrl).HasMaxLength(500);
        builder.Property(a => a.CvUrl).HasMaxLength(500);

        // --- 1:1 RELATIONSHIPS (BIO SEGMENTS) ---
        // We use DeleteBehavior.NoAction to prevent MSSQL 'Multiple Cascade Paths' errors.
        // We use explicit navigation properties to prevent 'ApplicantId1' shadow properties.

        builder.HasOne(a => a.PersonalInfo)
               .WithOne(p => p.Applicant)
               .HasForeignKey<ApplicantPersonalInfo>(p => p.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.FamilySummary)
               .WithOne(f => f.Applicant)
               .HasForeignKey<ApplicantFamilySummary>(f => f.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.FinancialDetail)
               .WithOne(f => f.Applicant)
               .HasForeignKey<ApplicantFinancialDetail>(f => f.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(a => a.MilitaryDetail)
               .WithOne(m => m.Applicant)
               .HasForeignKey<ApplicantMilitaryDetail>(m => m.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);


        // --- 1:N RELATIONSHIPS (HISTORICAL DATA) ---
        // Explicitly linking child.Applicant to parent so EF does NOT create ApplicantId1

        builder.HasMany(a => a.Educations)
               .WithOne(e => e.Applicant)
               .HasForeignKey(e => e.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Experiences)
               .WithOne(e => e.Applicant)
               .HasForeignKey(e => e.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Siblings)
               .WithOne(s => s.Applicant)
               .HasForeignKey(s => s.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.InternalRelatives)
               .WithOne(r => r.Applicant)
               .HasForeignKey(r => r.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Certifications)
               .WithOne(c => c.Applicant)
               .HasForeignKey(c => c.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Skills)
               .WithOne(s => s.Applicant)
               .HasForeignKey(s => s.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Achievements)
               .WithOne(ac => ac.Applicant)
               .HasForeignKey(ac => ac.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(a => a.Documents)
               .WithOne(d => d.Applicant)
               .HasForeignKey(d => d.ApplicantId)
               .OnDelete(DeleteBehavior.NoAction);
    }
}