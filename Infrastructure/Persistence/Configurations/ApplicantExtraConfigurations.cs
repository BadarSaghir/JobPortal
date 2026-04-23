using Career635.Domain.Entities.Applicants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApplicantDocumentConfiguration : IEntityTypeConfiguration<ApplicantDocument>
{
    public void Configure(EntityTypeBuilder<ApplicantDocument> builder)
    {
        builder.ToTable("ApplicantDocuments");
        builder.Property(d => d.DocumentType).IsRequired().HasMaxLength(100);
        builder.Property(d => d.FileUrl).IsRequired().HasMaxLength(500);
      builder.HasOne(d => d.Applicant)
           .WithMany(a => a.Documents)
           .HasForeignKey(d => d.ApplicantId) // Point exactly to ApplicantId
           .OnDelete(DeleteBehavior.NoAction); // Crucial for MSSQL
    }
}

public class ApplicantSkillConfiguration : IEntityTypeConfiguration<ApplicantSkill>
{
    public void Configure(EntityTypeBuilder<ApplicantSkill> builder)
    {
        builder.ToTable("ApplicantSkills");
        builder.Property(s => s.SkillName).IsRequired().HasMaxLength(100);
        builder.HasOne<Applicant>().WithMany(a => a.Skills).HasForeignKey(s => s.ApplicantId);
    }
}

public class ApplicantCertificationConfiguration : IEntityTypeConfiguration<ApplicantCertification>
{
    public void Configure(EntityTypeBuilder<ApplicantCertification> builder)
    {
        builder.ToTable("ApplicantCertifications");
        builder.Property(c => c.CertificateName).IsRequired().HasMaxLength(200);
        builder.HasOne<Applicant>().WithMany(a => a.Certifications).HasForeignKey(c => c.ApplicantId);
    }
}