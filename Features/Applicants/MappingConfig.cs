using Mapster;
using Career635.Areas.Candidate.Models;
using Career635.Domain.Entities.Applicants;

namespace Career635.Features.Applicants;

public class ApplicantMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        // 1. MAIN APPLICANT MAPPING
        config.NewConfig<ApplicationSubmitViewModel, Applicant>()
            // Map base properties
            .Map(dest => dest.FullName, src => src.FullName)
            .Map(dest => dest.CNICNumber, src => src.CNICNumber)
            .Map(dest => dest.AppliedAt, src => DateTime.UtcNow)
            
            // Map 1:1 Relations (We pass the entire source object to create the sub-entity)
            .Map(dest => dest.PersonalInfo, src => src)
            .Map(dest => dest.FamilySummary, src => src)
            .Map(dest => dest.FinancialDetail, src => src)
            .Map(dest => dest.MilitaryDetail, src => src)
            
            // Map 1:N Collections
            // Note: Filter out empty rows that the UI might submit (where critical fields are missing)
            .Map(dest => dest.Educations, src => src.Educations.Where(x => !string.IsNullOrEmpty(x.Qualification)))
            .Map(dest => dest.Experiences, src => src.Experiences.Where(x => !string.IsNullOrEmpty(x.OrganizationName)))
            .Map(dest => dest.Siblings, src => src.Siblings.Where(x => !string.IsNullOrEmpty(x.Name)))
            .Map(dest => dest.InternalRelatives, src => src.InternalRelatives.Where(x => !string.IsNullOrEmpty(x.RelativeName)))
            .Map(dest => dest.Skills, src => src.Skills.Where(x => !string.IsNullOrEmpty(x.SkillName)))
            .Map(dest => dest.Certifications, src => src.Certifications.Where(x => !string.IsNullOrEmpty(x.CertificateName)))
            .Map(dest => dest.Achievements, src => src.Achievements.Where(x => !string.IsNullOrEmpty(x.Title)));

        // 2. 1-to-1 SUB-ENTITY CONFIGURATIONS
        // These tell Mapster how to translate 'ApplicationSubmitViewModel' into the nested EF Entities
        
        config.NewConfig<ApplicationSubmitViewModel, ApplicantPersonalInfo>()
            .Map(dest => dest.CandidateType, src => src.CandidateType)
            .Map(dest => dest.FatherName, src => src.FatherName)
            .Map(dest => dest.FatherCNIC, src => src.FatherCNIC)
            .Map(dest => dest.DateOfBirth, src => src.DateOfBirth)
            .Map(dest => dest.MaritalStatus, src => src.MaritalStatus)
            .Map(dest => dest.Religion, src => src.Religion)
            .Map(dest => dest.Caste, src => src.Caste)
            .Map(dest => dest.Sect, src => src.Sect)
            .Map(dest => dest.ContactNo, src => src.ContactNo)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.PECNumber, src => src.PECNumber)
            .Map(dest => dest.PresentAddress, src => src.PresentAddress)
            .Map(dest => dest.PermanentAddress, src => src.PermanentAddress);

        config.NewConfig<ApplicationSubmitViewModel, ApplicantFamilySummary>()
            .Map(dest => dest.BrothersTotal, src => src.BrothersTotal)
            .Map(dest => dest.BrothersMarried, src => src.BrothersMarried)
            .Map(dest => dest.BrothersUnmarried, src => src.BrothersUnmarried)
            .Map(dest => dest.SistersTotal, src => src.SistersTotal)
            .Map(dest => dest.SistersMarried, src => src.SistersMarried)
            .Map(dest => dest.SistersUnmarried, src => src.SistersUnmarried)
            .Map(dest => dest.ChildrenTotal, src => src.ChildrenTotal)
            .Map(dest => dest.ChildrenMarried, src => src.ChildrenMarried)
            .Map(dest => dest.ChildrenUnmarried, src => src.ChildrenUnmarried);

        config.NewConfig<ApplicationSubmitViewModel, ApplicantFinancialDetail>()
            .Map(dest => dest.CurrentSalary, src => src.CurrentSalary)
            .Map(dest => dest.ExpectedSalary, src => src.ExpectedSalary)
            .Map(dest => dest.OtherBenefits, src => src.OtherBenefits)
            .Map(dest => dest.FamilyIncomeDetail, src => src.FamilyIncomeDetail);

        config.NewConfig<ApplicationSubmitViewModel, ApplicantMilitaryDetail>()
            .Map(dest => dest.ArmyNumber, src => src.ArmyNumber)
            .Map(dest => dest.ArmyUnit, src => src.ArmyUnit)
            .Map(dest => dest.ArmyCharacter, src => src.ArmyCharacter)
            .Map(dest => dest.ArmyPayScale, src => src.ArmyPayScale);

        // 3. COLLECTION ENTRY CONFIGURATIONS (Auto-mapped by property names, but explicit is safer)
        config.NewConfig<EducationEntry, ApplicantEducation>();
        config.NewConfig<ExperienceEntry, ApplicantExperience>();
        config.NewConfig<SiblingEntry, ApplicantSibling>();
        config.NewConfig<InternalRelativeEntry, ApplicantInternalRelative>();
        config.NewConfig<SkillEntry, ApplicantSkill>();
        config.NewConfig<CertificationEntry, ApplicantCertification>();
        config.NewConfig<AchievementEntry, ApplicantAchievement>();
        config.NewConfig<DocumentUploadEntry, ApplicantDocument>();
        
    }
}