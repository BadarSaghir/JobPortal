// Step 1 & 2: Personal Info
using Career635.Domain.Entities.Applicants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class ApplicantPersonalInfoConfiguration : IEntityTypeConfiguration<ApplicantPersonalInfo>
{
    public void Configure(EntityTypeBuilder<ApplicantPersonalInfo> builder)
    {
        builder.ToTable("ApplicantPersonalInfos");
        builder.Property(p => p.FatherName).IsRequired().HasMaxLength(200);
        builder.Property(p => p.ContactNo).IsRequired().HasMaxLength(20);
        
        // Structured Address Links (using IDs from the Location system)
  
    }
}

// Step 5: Financials
public class ApplicantFinancialConfiguration : IEntityTypeConfiguration<ApplicantFinancialDetail>
{
    public void Configure(EntityTypeBuilder<ApplicantFinancialDetail> builder)
    {
        builder.ToTable("ApplicantFinancialDetails");
        builder.Property(f => f.CurrentSalary).HasPrecision(18, 2);
        builder.Property(f => f.ExpectedSalary).HasPrecision(18, 2);
        builder.Property(f => f.FamilyIncomeDetail).HasMaxLength(100);
    }
}

// Step 7: Military
public class ApplicantMilitaryConfiguration : IEntityTypeConfiguration<ApplicantMilitaryDetail>
{
    public void Configure(EntityTypeBuilder<ApplicantMilitaryDetail> builder)
    {
        builder.ToTable("ApplicantMilitaryDetails");
        builder.Property(m => m.ArmyNumber).HasMaxLength(50);
        builder.Property(m => m.ArmyUnit).HasMaxLength(100);
    }
}