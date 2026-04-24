IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Applicants] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(200) NOT NULL,
        [CNICNumber] nvarchar(15) NOT NULL,
        [PassportImageUrl] nvarchar(500) NULL,
        [CvUrl] nvarchar(500) NULL,
        [TrackingCode] nvarchar(20) NOT NULL,
        [AppliedAt] datetime2 NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Applicants] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Countries] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Countries] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [DegreeLevels] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(max) NOT NULL,
        [LevelOrder] int NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_DegreeLevels] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Departments] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [Code] nvarchar(20) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Departments] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Designations] (
        [Id] uniqueidentifier NOT NULL,
        [Title] nvarchar(100) NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Designations] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [PayScales] (
        [Id] uniqueidentifier NOT NULL,
        [Grade] nvarchar(50) NOT NULL,
        [Description] nvarchar(max) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_PayScales] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Permissions] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(100) NOT NULL,
        [DisplayName] nvarchar(150) NOT NULL,
        [Module] nvarchar(max) NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Permissions] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [RecruitmentCampaigns] (
        [Id] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [CampaignCode] nvarchar(50) NOT NULL,
        [IsActive] bit NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_RecruitmentCampaigns] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Roles] (
        [Id] uniqueidentifier NOT NULL,
        [Description] nvarchar(500) NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        [Name] nvarchar(256) NULL,
        [NormalizedName] nvarchar(256) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        CONSTRAINT [PK_Roles] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantAchievements] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Description] nvarchar(max) NULL,
        [DateReceived] datetime2 NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantAchievements] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantAchievements_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantCertifications] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [CertificateName] nvarchar(200) NOT NULL,
        [IssuingBody] nvarchar(max) NOT NULL,
        [ExpiryDate] datetime2 NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantCertifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantCertifications_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantDocuments] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [DocumentType] nvarchar(100) NOT NULL,
        [FileUrl] nvarchar(500) NOT NULL,
        [UploadedAt] datetimeoffset NOT NULL,
        [ApplicantId1] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantDocuments] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantDocuments_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]),
        CONSTRAINT [FK_ApplicantDocuments_Applicants_ApplicantId1] FOREIGN KEY ([ApplicantId1]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantExperiences] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [OrganizationName] nvarchar(250) NOT NULL,
        [Designation] nvarchar(150) NOT NULL,
        [KeyResponsibilities] nvarchar(2000) NULL,
        [FromDate] datetime2 NOT NULL,
        [ToDate] datetime2 NULL,
        [ApplicantId1] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantExperiences] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantExperiences_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]),
        CONSTRAINT [FK_ApplicantExperiences_Applicants_ApplicantId1] FOREIGN KEY ([ApplicantId1]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantFamilySummaries] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [BrothersTotal] int NOT NULL,
        [BrothersMarried] int NOT NULL,
        [BrothersUnmarried] int NOT NULL,
        [SistersTotal] int NOT NULL,
        [SistersMarried] int NOT NULL,
        [SistersUnmarried] int NOT NULL,
        [ChildrenTotal] int NOT NULL,
        [ChildrenMarried] int NOT NULL,
        [ChildrenUnmarried] int NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantFamilySummaries] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantFamilySummaries_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantFinancialDetails] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [CurrentSalary] decimal(18,2) NULL,
        [ExpectedSalary] decimal(18,2) NULL,
        [OtherBenefits] nvarchar(max) NULL,
        [FamilyIncomeDetail] nvarchar(100) NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantFinancialDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantFinancialDetails_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantInternalRelatives] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NULL,
        [RelativeName] nvarchar(200) NOT NULL,
        [Designation] nvarchar(max) NOT NULL,
        [PayScale] nvarchar(max) NOT NULL,
        [Department] nvarchar(max) NOT NULL,
        [ApplicantId1] uniqueidentifier NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantInternalRelatives] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantInternalRelatives_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]),
        CONSTRAINT [FK_ApplicantInternalRelatives_Applicants_ApplicantId1] FOREIGN KEY ([ApplicantId1]) REFERENCES [Applicants] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantMilitaryDetails] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [ArmyNumber] nvarchar(50) NULL,
        [ArmyUnit] nvarchar(100) NULL,
        [ArmyCharacter] nvarchar(max) NULL,
        [ArmyPayScale] nvarchar(max) NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantMilitaryDetails] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantMilitaryDetails_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantPersonalInfos] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [CandidateType] nvarchar(max) NOT NULL,
        [FatherName] nvarchar(200) NOT NULL,
        [FatherCNIC] nvarchar(max) NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [MaritalStatus] nvarchar(max) NOT NULL,
        [Religion] nvarchar(max) NOT NULL,
        [Caste] nvarchar(max) NULL,
        [Sect] nvarchar(max) NULL,
        [ContactNo] nvarchar(20) NOT NULL,
        [Email] nvarchar(max) NULL,
        [PECNumber] nvarchar(max) NULL,
        [PresentAddress] nvarchar(max) NOT NULL,
        [PermanentAddress] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantPersonalInfos] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantPersonalInfos_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantSiblings] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [Name] nvarchar(200) NOT NULL,
        [CNIC] nvarchar(15) NULL,
        [DateOfBirth] datetime2 NOT NULL,
        [Occupation] nvarchar(max) NULL,
        [Organization] nvarchar(max) NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantSiblings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantSiblings_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantSkills] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [SkillName] nvarchar(100) NOT NULL,
        [Proficiency] nvarchar(max) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantSkills] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantSkills_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Provinces] (
        [Id] uniqueidentifier NOT NULL,
        [CountryId] uniqueidentifier NULL,
        [Name] nvarchar(100) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Provinces] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Provinces_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicantEducations] (
        [Id] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [DegreeLevelId] uniqueidentifier NOT NULL,
        [Qualification] nvarchar(100) NOT NULL,
        [MajorField] nvarchar(max) NOT NULL,
        [BoardUniversity] nvarchar(200) NOT NULL,
        [CgpaPercentage] nvarchar(max) NOT NULL,
        [FromDate] datetime2 NOT NULL,
        [ToDate] datetime2 NOT NULL,
        [ApplicantId1] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicantEducations] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicantEducations_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]),
        CONSTRAINT [FK_ApplicantEducations_Applicants_ApplicantId1] FOREIGN KEY ([ApplicantId1]) REFERENCES [Applicants] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ApplicantEducations_DegreeLevels_DegreeLevelId] FOREIGN KEY ([DegreeLevelId]) REFERENCES [DegreeLevels] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Users] (
        [Id] uniqueidentifier NOT NULL,
        [FullName] nvarchar(200) NOT NULL,
        [DesignationId] uniqueidentifier NULL,
        [DepartmentId] uniqueidentifier NULL,
        [PayScaleId] uniqueidentifier NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        [UserName] nvarchar(256) NULL,
        [NormalizedUserName] nvarchar(256) NULL,
        [Email] nvarchar(256) NULL,
        [NormalizedEmail] nvarchar(256) NULL,
        [EmailConfirmed] bit NOT NULL,
        [PasswordHash] nvarchar(max) NULL,
        [SecurityStamp] nvarchar(max) NULL,
        [ConcurrencyStamp] nvarchar(max) NULL,
        [PhoneNumber] nvarchar(max) NULL,
        [PhoneNumberConfirmed] bit NOT NULL,
        [TwoFactorEnabled] bit NOT NULL,
        [LockoutEnd] datetimeoffset NULL,
        [LockoutEnabled] bit NOT NULL,
        [AccessFailedCount] int NOT NULL,
        CONSTRAINT [PK_Users] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Users_Departments_DepartmentId] FOREIGN KEY ([DepartmentId]) REFERENCES [Departments] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Users_Designations_DesignationId] FOREIGN KEY ([DesignationId]) REFERENCES [Designations] ([Id]) ON DELETE NO ACTION,
        CONSTRAINT [FK_Users_PayScales_PayScaleId] FOREIGN KEY ([PayScaleId]) REFERENCES [PayScales] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [JobOpenings] (
        [Id] uniqueidentifier NOT NULL,
        [CampaignId] uniqueidentifier NULL,
        [Title] nvarchar(256) NOT NULL,
        [Department] nvarchar(150) NOT NULL,
        [Description] nvarchar(max) NOT NULL,
        [Requirements] nvarchar(max) NOT NULL,
        [LocationType] nvarchar(50) NULL,
        [WorkLocation] nvarchar(150) NULL,
        [MinAge] int NULL,
        [MaxAge] int NULL,
        [RequiredExperienceYears] decimal(18,2) NOT NULL,
        [MinEducationLevel] nvarchar(max) NOT NULL,
        [RequiredMajorField] nvarchar(max) NULL,
        [IsPecRequired] bit NOT NULL,
        [Status] nvarchar(50) NOT NULL,
        [PostedAt] datetime2 NOT NULL,
        [ExpiresAt] datetime2 NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_JobOpenings] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_JobOpenings_RecruitmentCampaigns_CampaignId] FOREIGN KEY ([CampaignId]) REFERENCES [RecruitmentCampaigns] ([Id]) ON DELETE SET NULL
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [AspNetRoleClaims] (
        [Id] int NOT NULL IDENTITY,
        [RoleId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetRoleClaims_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [RolePermissions] (
        [RoleId] uniqueidentifier NOT NULL,
        [PermissionId] uniqueidentifier NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_RolePermissions] PRIMARY KEY ([RoleId], [PermissionId]),
        CONSTRAINT [FK_RolePermissions_Permissions_PermissionId] FOREIGN KEY ([PermissionId]) REFERENCES [Permissions] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_RolePermissions_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Districts] (
        [Id] uniqueidentifier NOT NULL,
        [ProvinceId] uniqueidentifier NULL,
        [Name] nvarchar(100) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Districts] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Districts_Provinces_ProvinceId] FOREIGN KEY ([ProvinceId]) REFERENCES [Provinces] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [AspNetUserClaims] (
        [Id] int NOT NULL IDENTITY,
        [UserId] uniqueidentifier NOT NULL,
        [ClaimType] nvarchar(max) NULL,
        [ClaimValue] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_AspNetUserClaims_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [AspNetUserLogins] (
        [LoginProvider] nvarchar(450) NOT NULL,
        [ProviderKey] nvarchar(450) NOT NULL,
        [ProviderDisplayName] nvarchar(max) NULL,
        [UserId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
        CONSTRAINT [FK_AspNetUserLogins_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [AspNetUserRoles] (
        [UserId] uniqueidentifier NOT NULL,
        [RoleId] uniqueidentifier NOT NULL,
        CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
        CONSTRAINT [FK_AspNetUserRoles_Roles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Roles] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_AspNetUserRoles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [AspNetUserTokens] (
        [UserId] uniqueidentifier NOT NULL,
        [LoginProvider] nvarchar(450) NOT NULL,
        [Name] nvarchar(450) NOT NULL,
        [Value] nvarchar(max) NULL,
        CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
        CONSTRAINT [FK_AspNetUserTokens_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [JobApplications] (
        [Id] uniqueidentifier NOT NULL,
        [JobOpeningId] uniqueidentifier NOT NULL,
        [ApplicantId] uniqueidentifier NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [MatchScore] decimal(5,2) NOT NULL,
        [RecruiterRemarks] nvarchar(max) NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_JobApplications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_JobApplications_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]),
        CONSTRAINT [FK_JobApplications_JobOpenings_JobOpeningId] FOREIGN KEY ([JobOpeningId]) REFERENCES [JobOpenings] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [JobSkillRequirements] (
        [Id] uniqueidentifier NOT NULL,
        [JobOpeningId] uniqueidentifier NOT NULL,
        [SkillName] nvarchar(100) NOT NULL,
        [IsMandatory] bit NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_JobSkillRequirements] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_JobSkillRequirements_JobOpenings_JobOpeningId] FOREIGN KEY ([JobOpeningId]) REFERENCES [JobOpenings] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Tehsils] (
        [Id] uniqueidentifier NOT NULL,
        [DistrictId] uniqueidentifier NULL,
        [Name] nvarchar(100) NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Tehsils] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Tehsils_Districts_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [Districts] ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [ApplicationStatusHistory] (
        [Id] uniqueidentifier NOT NULL,
        [JobApplicationId] uniqueidentifier NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [Remarks] nvarchar(max) NULL,
        [ChangedByUserId] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_ApplicationStatusHistory] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_ApplicationStatusHistory_JobApplications_JobApplicationId] FOREIGN KEY ([JobApplicationId]) REFERENCES [JobApplications] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_ApplicationStatusHistory_Users_ChangedByUserId] FOREIGN KEY ([ChangedByUserId]) REFERENCES [Users] ([Id]) ON DELETE NO ACTION
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE TABLE [Addresses] (
        [Id] uniqueidentifier NOT NULL,
        [CountryId] uniqueidentifier NOT NULL,
        [ProvinceId] uniqueidentifier NOT NULL,
        [DistrictId] uniqueidentifier NOT NULL,
        [TehsilId] uniqueidentifier NOT NULL,
        [StreetAddress] nvarchar(max) NOT NULL,
        [CityId] uniqueidentifier NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_Addresses] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_Addresses_Countries_CountryId] FOREIGN KEY ([CountryId]) REFERENCES [Countries] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Addresses_Districts_DistrictId] FOREIGN KEY ([DistrictId]) REFERENCES [Districts] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Addresses_Provinces_ProvinceId] FOREIGN KEY ([ProvinceId]) REFERENCES [Provinces] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_Addresses_Tehsils_CityId] FOREIGN KEY ([CityId]) REFERENCES [Tehsils] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Addresses_CityId] ON [Addresses] ([CityId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Addresses_CountryId] ON [Addresses] ([CountryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Addresses_DistrictId] ON [Addresses] ([DistrictId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Addresses_ProvinceId] ON [Addresses] ([ProvinceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantAchievements_ApplicantId] ON [ApplicantAchievements] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantCertifications_ApplicantId] ON [ApplicantCertifications] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantDocuments_ApplicantId] ON [ApplicantDocuments] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantDocuments_ApplicantId1] ON [ApplicantDocuments] ([ApplicantId1]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantEducations_ApplicantId] ON [ApplicantEducations] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantEducations_ApplicantId1] ON [ApplicantEducations] ([ApplicantId1]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantEducations_DegreeLevelId] ON [ApplicantEducations] ([DegreeLevelId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantExperiences_ApplicantId] ON [ApplicantExperiences] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantExperiences_ApplicantId1] ON [ApplicantExperiences] ([ApplicantId1]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ApplicantFamilySummaries_ApplicantId] ON [ApplicantFamilySummaries] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ApplicantFinancialDetails_ApplicantId] ON [ApplicantFinancialDetails] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantInternalRelatives_ApplicantId] ON [ApplicantInternalRelatives] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantInternalRelatives_ApplicantId1] ON [ApplicantInternalRelatives] ([ApplicantId1]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ApplicantMilitaryDetails_ApplicantId] ON [ApplicantMilitaryDetails] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE UNIQUE INDEX [IX_ApplicantPersonalInfos_ApplicantId] ON [ApplicantPersonalInfos] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Applicants_CNICNumber] ON [Applicants] ([CNICNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Applicants_TrackingCode] ON [Applicants] ([TrackingCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantSiblings_ApplicantId] ON [ApplicantSiblings] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicantSkills_ApplicantId] ON [ApplicantSkills] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicationStatusHistory_ChangedByUserId] ON [ApplicationStatusHistory] ([ChangedByUserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_ApplicationStatusHistory_JobApplicationId] ON [ApplicationStatusHistory] ([JobApplicationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Districts_ProvinceId] ON [Districts] ([ProvinceId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_JobApplications_ApplicantId] ON [JobApplications] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_JobApplications_JobOpeningId] ON [JobApplications] ([JobOpeningId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_JobOpenings_CampaignId] ON [JobOpenings] ([CampaignId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_JobOpenings_ExpiresAt] ON [JobOpenings] ([ExpiresAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_JobOpenings_PostedAt] ON [JobOpenings] ([PostedAt]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_JobOpenings_Status] ON [JobOpenings] ([Status]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_JobSkillRequirements_JobOpeningId] ON [JobSkillRequirements] ([JobOpeningId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE UNIQUE INDEX [IX_Permissions_Name] ON [Permissions] ([Name]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Provinces_CountryId] ON [Provinces] ([CountryId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE UNIQUE INDEX [IX_RecruitmentCampaigns_CampaignCode] ON [RecruitmentCampaigns] ([CampaignCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_RolePermissions_PermissionId] ON [RolePermissions] ([PermissionId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [RoleNameIndex] ON [Roles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Tehsils_DistrictId] ON [Tehsils] ([DistrictId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [EmailIndex] ON [Users] ([NormalizedEmail]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Users_DepartmentId] ON [Users] ([DepartmentId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Users_DesignationId] ON [Users] ([DesignationId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    CREATE INDEX [IX_Users_PayScaleId] ON [Users] ([PayScaleId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [UserNameIndex] ON [Users] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421074347_Addres'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260421074347_Addres', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantAchievements] DROP CONSTRAINT [FK_ApplicantAchievements_Applicants_ApplicantId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantCertifications] DROP CONSTRAINT [FK_ApplicantCertifications_Applicants_ApplicantId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantDocuments] DROP CONSTRAINT [FK_ApplicantDocuments_Applicants_ApplicantId1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantEducations] DROP CONSTRAINT [FK_ApplicantEducations_Applicants_ApplicantId1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantExperiences] DROP CONSTRAINT [FK_ApplicantExperiences_Applicants_ApplicantId1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantInternalRelatives] DROP CONSTRAINT [FK_ApplicantInternalRelatives_Applicants_ApplicantId1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantSkills] DROP CONSTRAINT [FK_ApplicantSkills_Applicants_ApplicantId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DROP INDEX [IX_ApplicantPersonalInfos_ApplicantId] ON [ApplicantPersonalInfos];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DROP INDEX [IX_ApplicantInternalRelatives_ApplicantId1] ON [ApplicantInternalRelatives];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DROP INDEX [IX_ApplicantExperiences_ApplicantId1] ON [ApplicantExperiences];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DROP INDEX [IX_ApplicantEducations_ApplicantId1] ON [ApplicantEducations];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DROP INDEX [IX_ApplicantDocuments_ApplicantId1] ON [ApplicantDocuments];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DECLARE @var nvarchar(max);
    SELECT @var = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[JobOpenings]') AND [c].[name] = N'Department');
    IF @var IS NOT NULL EXEC(N'ALTER TABLE [JobOpenings] DROP CONSTRAINT ' + @var + ';');
    ALTER TABLE [JobOpenings] DROP COLUMN [Department];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DECLARE @var1 nvarchar(max);
    SELECT @var1 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicantInternalRelatives]') AND [c].[name] = N'ApplicantId1');
    IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [ApplicantInternalRelatives] DROP CONSTRAINT ' + @var1 + ';');
    ALTER TABLE [ApplicantInternalRelatives] DROP COLUMN [ApplicantId1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DECLARE @var2 nvarchar(max);
    SELECT @var2 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicantExperiences]') AND [c].[name] = N'ApplicantId1');
    IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [ApplicantExperiences] DROP CONSTRAINT ' + @var2 + ';');
    ALTER TABLE [ApplicantExperiences] DROP COLUMN [ApplicantId1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DECLARE @var3 nvarchar(max);
    SELECT @var3 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicantEducations]') AND [c].[name] = N'ApplicantId1');
    IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [ApplicantEducations] DROP CONSTRAINT ' + @var3 + ';');
    ALTER TABLE [ApplicantEducations] DROP COLUMN [ApplicantId1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DECLARE @var4 nvarchar(max);
    SELECT @var4 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicantDocuments]') AND [c].[name] = N'ApplicantId1');
    IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [ApplicantDocuments] DROP CONSTRAINT ' + @var4 + ';');
    ALTER TABLE [ApplicantDocuments] DROP COLUMN [ApplicantId1];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DECLARE @var5 nvarchar(max);
    SELECT @var5 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicantPersonalInfos]') AND [c].[name] = N'ApplicantId');
    IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [ApplicantPersonalInfos] DROP CONSTRAINT ' + @var5 + ';');
    ALTER TABLE [ApplicantPersonalInfos] ALTER COLUMN [ApplicantId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DROP INDEX [IX_ApplicantInternalRelatives_ApplicantId] ON [ApplicantInternalRelatives];
    DECLARE @var6 nvarchar(max);
    SELECT @var6 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicantInternalRelatives]') AND [c].[name] = N'ApplicantId');
    IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [ApplicantInternalRelatives] DROP CONSTRAINT ' + @var6 + ';');
    EXEC(N'UPDATE [ApplicantInternalRelatives] SET [ApplicantId] = ''00000000-0000-0000-0000-000000000000'' WHERE [ApplicantId] IS NULL');
    ALTER TABLE [ApplicantInternalRelatives] ALTER COLUMN [ApplicantId] uniqueidentifier NOT NULL;
    ALTER TABLE [ApplicantInternalRelatives] ADD DEFAULT '00000000-0000-0000-0000-000000000000' FOR [ApplicantId];
    CREATE INDEX [IX_ApplicantInternalRelatives_ApplicantId] ON [ApplicantInternalRelatives] ([ApplicantId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DECLARE @var7 nvarchar(max);
    SELECT @var7 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicantEducations]') AND [c].[name] = N'DegreeLevelId');
    IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [ApplicantEducations] DROP CONSTRAINT ' + @var7 + ';');
    ALTER TABLE [ApplicantEducations] ALTER COLUMN [DegreeLevelId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    DECLARE @var8 nvarchar(max);
    SELECT @var8 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[ApplicantDocuments]') AND [c].[name] = N'ApplicantId');
    IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [ApplicantDocuments] DROP CONSTRAINT ' + @var8 + ';');
    ALTER TABLE [ApplicantDocuments] ALTER COLUMN [ApplicantId] uniqueidentifier NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    EXEC(N'CREATE UNIQUE INDEX [IX_ApplicantPersonalInfos_ApplicantId] ON [ApplicantPersonalInfos] ([ApplicantId]) WHERE [ApplicantId] IS NOT NULL');
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantAchievements] ADD CONSTRAINT [FK_ApplicantAchievements_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantCertifications] ADD CONSTRAINT [FK_ApplicantCertifications_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    ALTER TABLE [ApplicantSkills] ADD CONSTRAINT [FK_ApplicantSkills_Applicants_ApplicantId] FOREIGN KEY ([ApplicantId]) REFERENCES [Applicants] ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421085252_Ad0dres'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260421085252_Ad0dres', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421101234_Added_columns'
)
BEGIN
    ALTER TABLE [JobOpenings] ADD [Benefits] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421101234_Added_columns'
)
BEGIN
    ALTER TABLE [JobOpenings] ADD [EmploymentType] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421101234_Added_columns'
)
BEGIN
    ALTER TABLE [JobOpenings] ADD [IsFeatured] bit NOT NULL DEFAULT CAST(0 AS bit);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421101234_Added_columns'
)
BEGIN
    ALTER TABLE [JobOpenings] ADD [JobCategory] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421101234_Added_columns'
)
BEGIN
    ALTER TABLE [JobOpenings] ADD [JobSlug] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421101234_Added_columns'
)
BEGIN
    ALTER TABLE [JobOpenings] ADD [SalaryGrade] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421101234_Added_columns'
)
BEGIN
    ALTER TABLE [JobOpenings] ADD [TotalPositions] int NOT NULL DEFAULT 0;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421101234_Added_columns'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260421101234_Added_columns', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421113421_Added_columns_appliedAt'
)
BEGIN
    DECLARE @var9 nvarchar(max);
    SELECT @var9 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[JobOpenings]') AND [c].[name] = N'TotalPositions');
    IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [JobOpenings] DROP CONSTRAINT ' + @var9 + ';');
    ALTER TABLE [JobOpenings] ALTER COLUMN [TotalPositions] int NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421113421_Added_columns_appliedAt'
)
BEGIN
    DECLARE @var10 nvarchar(max);
    SELECT @var10 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[JobOpenings]') AND [c].[name] = N'JobCategory');
    IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [JobOpenings] DROP CONSTRAINT ' + @var10 + ';');
    ALTER TABLE [JobOpenings] ALTER COLUMN [JobCategory] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421113421_Added_columns_appliedAt'
)
BEGIN
    DECLARE @var11 nvarchar(max);
    SELECT @var11 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[JobOpenings]') AND [c].[name] = N'EmploymentType');
    IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [JobOpenings] DROP CONSTRAINT ' + @var11 + ';');
    ALTER TABLE [JobOpenings] ALTER COLUMN [EmploymentType] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421113421_Added_columns_appliedAt'
)
BEGIN
    ALTER TABLE [JobApplications] ADD [AppliedAt] datetimeoffset NOT NULL DEFAULT '0001-01-01T00:00:00.0000000+00:00';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421113421_Added_columns_appliedAt'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260421113421_Added_columns_appliedAt', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421123859_Added_cnic_rm'
)
BEGIN
    DROP INDEX [IX_Applicants_CNICNumber] ON [Applicants];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421123859_Added_cnic_rm'
)
BEGIN
    ALTER TABLE [Applicants] ADD [UpdatedAt] datetime2 NOT NULL DEFAULT '0001-01-01T00:00:00.0000000';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421123859_Added_cnic_rm'
)
BEGIN
    CREATE INDEX [IX_Applicants_CNICNumber] ON [Applicants] ([CNICNumber]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421123859_Added_cnic_rm'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260421123859_Added_cnic_rm', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421151039_Added_cnic_geneder'
)
BEGIN
    ALTER TABLE [ApplicantPersonalInfos] ADD [Gender] nvarchar(max) NOT NULL DEFAULT N'';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421151039_Added_cnic_geneder'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260421151039_Added_cnic_geneder', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421160632_Added_inv'
)
BEGIN
    DROP INDEX [IX_Applicants_TrackingCode] ON [Applicants];
    DECLARE @var12 nvarchar(max);
    SELECT @var12 = QUOTENAME([d].[name])
    FROM [sys].[default_constraints] [d]
    INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
    WHERE ([d].[parent_object_id] = OBJECT_ID(N'[Applicants]') AND [c].[name] = N'TrackingCode');
    IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [Applicants] DROP CONSTRAINT ' + @var12 + ';');
    ALTER TABLE [Applicants] ALTER COLUMN [TrackingCode] nvarchar(50) NOT NULL;
    CREATE UNIQUE INDEX [IX_Applicants_TrackingCode] ON [Applicants] ([TrackingCode]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260421160632_Added_inv'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260421160632_Added_inv', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260422034727_data_fixed'
)
BEGIN
    ALTER TABLE [ApplicantSiblings] ADD [Designation] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260422034727_data_fixed'
)
BEGIN
    ALTER TABLE [ApplicantSiblings] ADD [Gender] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260422034727_data_fixed'
)
BEGIN
    ALTER TABLE [ApplicantSiblings] ADD [MaritalStatus] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260422034727_data_fixed'
)
BEGIN
    ALTER TABLE [ApplicantPersonalInfos] ADD [Accommodation] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260422034727_data_fixed'
)
BEGIN
    ALTER TABLE [ApplicantFinancialDetails] ADD [OtherFacilities] nvarchar(max) NULL;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260422034727_data_fixed'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260422034727_data_fixed', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260422050647_Added_colv'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260422050647_Added_colv', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423070247_FinalArchitecture'
)
BEGIN
    CREATE TABLE [CampaignExportTasks] (
        [Id] uniqueidentifier NOT NULL,
        [CampaignId] uniqueidentifier NOT NULL,
        [RequestedByUserId] uniqueidentifier NOT NULL,
        [Status] nvarchar(max) NOT NULL,
        [DownloadUrl] nvarchar(max) NULL,
        [ErrorMessage] nvarchar(max) NULL,
        [ProcessedAt] datetime2 NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_CampaignExportTasks] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_CampaignExportTasks_RecruitmentCampaigns_CampaignId] FOREIGN KEY ([CampaignId]) REFERENCES [RecruitmentCampaigns] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423070247_FinalArchitecture'
)
BEGIN
    CREATE INDEX [IX_CampaignExportTasks_CampaignId] ON [CampaignExportTasks] ([CampaignId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423070247_FinalArchitecture'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260423070247_FinalArchitecture', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423104654_AddApplicationsNavigation'
)
BEGIN
    ALTER TABLE [JobApplications] DROP CONSTRAINT [FK_JobApplications_JobOpenings_JobOpeningId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423104654_AddApplicationsNavigation'
)
BEGIN
    ALTER TABLE [JobApplications] ADD CONSTRAINT [FK_JobApplications_JobOpenings_JobOpeningId] FOREIGN KEY ([JobOpeningId]) REFERENCES [JobOpenings] ([Id]) ON DELETE NO ACTION;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260423104654_AddApplicationsNavigation'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260423104654_AddApplicationsNavigation', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424053523_Added_notication'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260424053523_Added_notication', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064345_Added_audit_notication'
)
BEGIN
    CREATE TABLE [auditLogs] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] nvarchar(max) NULL,
        [UserName] nvarchar(max) NULL,
        [Action] nvarchar(max) NOT NULL,
        [EntityName] nvarchar(max) NOT NULL,
        [EntityId] nvarchar(max) NULL,
        [IPAddress] nvarchar(max) NULL,
        [UserAgent] nvarchar(max) NULL,
        [Path] nvarchar(max) NULL,
        [OldValues] nvarchar(max) NULL,
        [NewValues] nvarchar(max) NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_auditLogs] PRIMARY KEY ([Id])
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064345_Added_audit_notication'
)
BEGIN
    CREATE TABLE [userNotifications] (
        [Id] uniqueidentifier NOT NULL,
        [UserId] uniqueidentifier NOT NULL,
        [Title] nvarchar(max) NOT NULL,
        [Message] nvarchar(max) NOT NULL,
        [ActionUrl] nvarchar(max) NULL,
        [Type] nvarchar(max) NOT NULL,
        [IsRead] bit NOT NULL,
        [CreatedAt] datetimeoffset NOT NULL,
        [IsDeleted] bit NOT NULL,
        [DeletedAt] datetimeoffset NULL,
        CONSTRAINT [PK_userNotifications] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_userNotifications_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE
    );
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064345_Added_audit_notication'
)
BEGIN
    CREATE INDEX [IX_userNotifications_UserId] ON [userNotifications] ([UserId]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064345_Added_audit_notication'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260424064345_Added_audit_notication', N'10.0.6');
END;

COMMIT;
GO

BEGIN TRANSACTION;
IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    ALTER TABLE [userNotifications] DROP CONSTRAINT [FK_userNotifications_Users_UserId];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    ALTER TABLE [userNotifications] DROP CONSTRAINT [PK_userNotifications];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    ALTER TABLE [auditLogs] DROP CONSTRAINT [PK_auditLogs];
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    EXEC sp_rename N'[userNotifications]', N'UserNotifications', 'OBJECT';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    EXEC sp_rename N'[auditLogs]', N'AuditLogs', 'OBJECT';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    EXEC sp_rename N'[UserNotifications].[IX_userNotifications_UserId]', N'IX_UserNotifications_UserId', 'INDEX';
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    ALTER TABLE [UserNotifications] ADD CONSTRAINT [PK_UserNotifications] PRIMARY KEY ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    ALTER TABLE [AuditLogs] ADD CONSTRAINT [PK_AuditLogs] PRIMARY KEY ([Id]);
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    ALTER TABLE [UserNotifications] ADD CONSTRAINT [FK_UserNotifications_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [Users] ([Id]) ON DELETE CASCADE;
END;

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20260424064730_Added_audit_notication_correction'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20260424064730_Added_audit_notication_correction', N'10.0.6');
END;

COMMIT;
GO

