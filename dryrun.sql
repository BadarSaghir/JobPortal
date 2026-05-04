CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Applicants" (
        "Id" uniqueidentifier NOT NULL,
        "FullName" nvarchar(200) NOT NULL,
        "CNICNumber" nvarchar(15) NOT NULL,
        "PassportImageUrl" nvarchar(500),
        "CvUrl" nvarchar(500),
        "TrackingCode" nvarchar(20) NOT NULL,
        "AppliedAt" datetime2 NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Applicants" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Countries" (
        "Id" uniqueidentifier NOT NULL,
        "Name" nvarchar(max) NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Countries" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "DegreeLevels" (
        "Id" uniqueidentifier NOT NULL,
        "Name" nvarchar(max) NOT NULL,
        "LevelOrder" int NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_DegreeLevels" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Departments" (
        "Id" uniqueidentifier NOT NULL,
        "Name" nvarchar(100) NOT NULL,
        "Code" nvarchar(20),
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Departments" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Designations" (
        "Id" uniqueidentifier NOT NULL,
        "Title" nvarchar(100) NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Designations" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "PayScales" (
        "Id" uniqueidentifier NOT NULL,
        "Grade" nvarchar(50) NOT NULL,
        "Description" nvarchar(max),
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_PayScales" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Permissions" (
        "Id" uniqueidentifier NOT NULL,
        "Name" nvarchar(100) NOT NULL,
        "DisplayName" nvarchar(150) NOT NULL,
        "Module" nvarchar(max) NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Permissions" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "RecruitmentCampaigns" (
        "Id" uniqueidentifier NOT NULL,
        "Name" nvarchar(200) NOT NULL,
        "CampaignCode" nvarchar(50) NOT NULL,
        "IsActive" bit NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_RecruitmentCampaigns" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Roles" (
        "Id" uniqueidentifier NOT NULL,
        "Description" nvarchar(500),
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        "Name" nvarchar(256),
        "NormalizedName" nvarchar(256),
        "ConcurrencyStamp" nvarchar(max),
        CONSTRAINT "PK_Roles" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantAchievements" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "Title" nvarchar(max) NOT NULL,
        "Description" nvarchar(max),
        "DateReceived" datetime2 NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantAchievements" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantAchievements_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantCertifications" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "CertificateName" nvarchar(200) NOT NULL,
        "IssuingBody" nvarchar(max) NOT NULL,
        "ExpiryDate" datetime2,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantCertifications" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantCertifications_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantDocuments" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "DocumentType" nvarchar(100) NOT NULL,
        "FileUrl" nvarchar(500) NOT NULL,
        "UploadedAt" datetimeoffset NOT NULL,
        "ApplicantId1" uniqueidentifier NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantDocuments" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantDocuments_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id"),
        CONSTRAINT "FK_ApplicantDocuments_Applicants_ApplicantId1" FOREIGN KEY ("ApplicantId1") REFERENCES "Applicants" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantExperiences" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "OrganizationName" nvarchar(250) NOT NULL,
        "Designation" nvarchar(150) NOT NULL,
        "KeyResponsibilities" nvarchar(2000),
        "FromDate" datetime2 NOT NULL,
        "ToDate" datetime2,
        "ApplicantId1" uniqueidentifier NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantExperiences" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantExperiences_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id"),
        CONSTRAINT "FK_ApplicantExperiences_Applicants_ApplicantId1" FOREIGN KEY ("ApplicantId1") REFERENCES "Applicants" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantFamilySummaries" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "BrothersTotal" int NOT NULL,
        "BrothersMarried" int NOT NULL,
        "BrothersUnmarried" int NOT NULL,
        "SistersTotal" int NOT NULL,
        "SistersMarried" int NOT NULL,
        "SistersUnmarried" int NOT NULL,
        "ChildrenTotal" int NOT NULL,
        "ChildrenMarried" int NOT NULL,
        "ChildrenUnmarried" int NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantFamilySummaries" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantFamilySummaries_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantFinancialDetails" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "CurrentSalary" decimal(18,2),
        "ExpectedSalary" decimal(18,2),
        "OtherBenefits" nvarchar(max),
        "FamilyIncomeDetail" nvarchar(100),
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantFinancialDetails" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantFinancialDetails_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantInternalRelatives" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier,
        "RelativeName" nvarchar(200) NOT NULL,
        "Designation" nvarchar(max) NOT NULL,
        "PayScale" nvarchar(max) NOT NULL,
        "Department" nvarchar(max) NOT NULL,
        "ApplicantId1" uniqueidentifier,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantInternalRelatives" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantInternalRelatives_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id"),
        CONSTRAINT "FK_ApplicantInternalRelatives_Applicants_ApplicantId1" FOREIGN KEY ("ApplicantId1") REFERENCES "Applicants" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantMilitaryDetails" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "ArmyNumber" nvarchar(50),
        "ArmyUnit" nvarchar(100),
        "ArmyCharacter" nvarchar(max),
        "ArmyPayScale" nvarchar(max),
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantMilitaryDetails" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantMilitaryDetails_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantPersonalInfos" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "CandidateType" nvarchar(max) NOT NULL,
        "FatherName" nvarchar(200) NOT NULL,
        "FatherCNIC" nvarchar(max),
        "DateOfBirth" datetime2 NOT NULL,
        "MaritalStatus" nvarchar(max) NOT NULL,
        "Religion" nvarchar(max) NOT NULL,
        "Caste" nvarchar(max),
        "Sect" nvarchar(max),
        "ContactNo" nvarchar(20) NOT NULL,
        "Email" nvarchar(max),
        "PECNumber" nvarchar(max),
        "PresentAddress" nvarchar(max) NOT NULL,
        "PermanentAddress" nvarchar(max) NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantPersonalInfos" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantPersonalInfos_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantSiblings" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "Name" nvarchar(200) NOT NULL,
        "CNIC" nvarchar(15),
        "DateOfBirth" datetime2 NOT NULL,
        "Occupation" nvarchar(max),
        "Organization" nvarchar(max),
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantSiblings" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantSiblings_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantSkills" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "SkillName" nvarchar(100) NOT NULL,
        "Proficiency" nvarchar(max) NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantSkills" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantSkills_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Provinces" (
        "Id" uniqueidentifier NOT NULL,
        "CountryId" uniqueidentifier,
        "Name" nvarchar(100) NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Provinces" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Provinces_Countries_CountryId" FOREIGN KEY ("CountryId") REFERENCES "Countries" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicantEducations" (
        "Id" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "DegreeLevelId" uniqueidentifier NOT NULL,
        "Qualification" nvarchar(100) NOT NULL,
        "MajorField" nvarchar(max) NOT NULL,
        "BoardUniversity" nvarchar(200) NOT NULL,
        "CgpaPercentage" nvarchar(max) NOT NULL,
        "FromDate" datetime2 NOT NULL,
        "ToDate" datetime2 NOT NULL,
        "ApplicantId1" uniqueidentifier NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicantEducations" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicantEducations_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id"),
        CONSTRAINT "FK_ApplicantEducations_Applicants_ApplicantId1" FOREIGN KEY ("ApplicantId1") REFERENCES "Applicants" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_ApplicantEducations_DegreeLevels_DegreeLevelId" FOREIGN KEY ("DegreeLevelId") REFERENCES "DegreeLevels" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Users" (
        "Id" uniqueidentifier NOT NULL,
        "FullName" nvarchar(200) NOT NULL,
        "DesignationId" uniqueidentifier,
        "DepartmentId" uniqueidentifier,
        "PayScaleId" uniqueidentifier,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        "UserName" nvarchar(256),
        "NormalizedUserName" nvarchar(256),
        "Email" nvarchar(256),
        "NormalizedEmail" nvarchar(256),
        "EmailConfirmed" bit NOT NULL,
        "PasswordHash" nvarchar(max),
        "SecurityStamp" nvarchar(max),
        "ConcurrencyStamp" nvarchar(max),
        "PhoneNumber" nvarchar(max),
        "PhoneNumberConfirmed" bit NOT NULL,
        "TwoFactorEnabled" bit NOT NULL,
        "LockoutEnd" datetimeoffset,
        "LockoutEnabled" bit NOT NULL,
        "AccessFailedCount" int NOT NULL,
        CONSTRAINT "PK_Users" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Users_Departments_DepartmentId" FOREIGN KEY ("DepartmentId") REFERENCES "Departments" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Users_Designations_DesignationId" FOREIGN KEY ("DesignationId") REFERENCES "Designations" ("Id") ON DELETE RESTRICT,
        CONSTRAINT "FK_Users_PayScales_PayScaleId" FOREIGN KEY ("PayScaleId") REFERENCES "PayScales" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "JobOpenings" (
        "Id" uniqueidentifier NOT NULL,
        "CampaignId" uniqueidentifier,
        "Title" nvarchar(256) NOT NULL,
        "Department" nvarchar(150) NOT NULL,
        "Description" nvarchar(max) NOT NULL,
        "Requirements" nvarchar(max) NOT NULL,
        "LocationType" nvarchar(50),
        "WorkLocation" nvarchar(150),
        "MinAge" int,
        "MaxAge" int,
        "RequiredExperienceYears" decimal(18,2) NOT NULL,
        "MinEducationLevel" nvarchar(max) NOT NULL,
        "RequiredMajorField" nvarchar(max),
        "IsPecRequired" bit NOT NULL,
        "Status" nvarchar(50) NOT NULL,
        "PostedAt" datetime2 NOT NULL,
        "ExpiresAt" datetime2 NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_JobOpenings" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_JobOpenings_RecruitmentCampaigns_CampaignId" FOREIGN KEY ("CampaignId") REFERENCES "RecruitmentCampaigns" ("Id") ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "AspNetRoleClaims" (
        "Id" int NOT NULL,
        "RoleId" uniqueidentifier NOT NULL,
        "ClaimType" nvarchar(max),
        "ClaimValue" nvarchar(max),
        CONSTRAINT "PK_AspNetRoleClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetRoleClaims_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "RolePermissions" (
        "RoleId" uniqueidentifier NOT NULL,
        "PermissionId" uniqueidentifier NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_RolePermissions" PRIMARY KEY ("RoleId", "PermissionId"),
        CONSTRAINT "FK_RolePermissions_Permissions_PermissionId" FOREIGN KEY ("PermissionId") REFERENCES "Permissions" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_RolePermissions_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Districts" (
        "Id" uniqueidentifier NOT NULL,
        "ProvinceId" uniqueidentifier,
        "Name" nvarchar(100) NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Districts" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Districts_Provinces_ProvinceId" FOREIGN KEY ("ProvinceId") REFERENCES "Provinces" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "AspNetUserClaims" (
        "Id" int NOT NULL,
        "UserId" uniqueidentifier NOT NULL,
        "ClaimType" nvarchar(max),
        "ClaimValue" nvarchar(max),
        CONSTRAINT "PK_AspNetUserClaims" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_AspNetUserClaims_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "AspNetUserLogins" (
        "LoginProvider" nvarchar(450) NOT NULL,
        "ProviderKey" nvarchar(450) NOT NULL,
        "ProviderDisplayName" nvarchar(max),
        "UserId" uniqueidentifier NOT NULL,
        CONSTRAINT "PK_AspNetUserLogins" PRIMARY KEY ("LoginProvider", "ProviderKey"),
        CONSTRAINT "FK_AspNetUserLogins_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "AspNetUserRoles" (
        "UserId" uniqueidentifier NOT NULL,
        "RoleId" uniqueidentifier NOT NULL,
        CONSTRAINT "PK_AspNetUserRoles" PRIMARY KEY ("UserId", "RoleId"),
        CONSTRAINT "FK_AspNetUserRoles_Roles_RoleId" FOREIGN KEY ("RoleId") REFERENCES "Roles" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_AspNetUserRoles_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "AspNetUserTokens" (
        "UserId" uniqueidentifier NOT NULL,
        "LoginProvider" nvarchar(450) NOT NULL,
        "Name" nvarchar(450) NOT NULL,
        "Value" nvarchar(max),
        CONSTRAINT "PK_AspNetUserTokens" PRIMARY KEY ("UserId", "LoginProvider", "Name"),
        CONSTRAINT "FK_AspNetUserTokens_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "JobApplications" (
        "Id" uniqueidentifier NOT NULL,
        "JobOpeningId" uniqueidentifier NOT NULL,
        "ApplicantId" uniqueidentifier NOT NULL,
        "Status" nvarchar(max) NOT NULL,
        "MatchScore" decimal(5,2) NOT NULL,
        "RecruiterRemarks" nvarchar(max),
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_JobApplications" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_JobApplications_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id"),
        CONSTRAINT "FK_JobApplications_JobOpenings_JobOpeningId" FOREIGN KEY ("JobOpeningId") REFERENCES "JobOpenings" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "JobSkillRequirements" (
        "Id" uniqueidentifier NOT NULL,
        "JobOpeningId" uniqueidentifier NOT NULL,
        "SkillName" nvarchar(100) NOT NULL,
        "IsMandatory" bit NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_JobSkillRequirements" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_JobSkillRequirements_JobOpenings_JobOpeningId" FOREIGN KEY ("JobOpeningId") REFERENCES "JobOpenings" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Tehsils" (
        "Id" uniqueidentifier NOT NULL,
        "DistrictId" uniqueidentifier,
        "Name" nvarchar(100) NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Tehsils" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Tehsils_Districts_DistrictId" FOREIGN KEY ("DistrictId") REFERENCES "Districts" ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "ApplicationStatusHistory" (
        "Id" uniqueidentifier NOT NULL,
        "JobApplicationId" uniqueidentifier NOT NULL,
        "Status" nvarchar(max) NOT NULL,
        "Remarks" nvarchar(max),
        "ChangedByUserId" uniqueidentifier NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_ApplicationStatusHistory" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_ApplicationStatusHistory_JobApplications_JobApplicationId" FOREIGN KEY ("JobApplicationId") REFERENCES "JobApplications" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_ApplicationStatusHistory_Users_ChangedByUserId" FOREIGN KEY ("ChangedByUserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE TABLE "Addresses" (
        "Id" uniqueidentifier NOT NULL,
        "CountryId" uniqueidentifier NOT NULL,
        "ProvinceId" uniqueidentifier NOT NULL,
        "DistrictId" uniqueidentifier NOT NULL,
        "TehsilId" uniqueidentifier NOT NULL,
        "StreetAddress" nvarchar(max) NOT NULL,
        "CityId" uniqueidentifier NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_Addresses" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_Addresses_Countries_CountryId" FOREIGN KEY ("CountryId") REFERENCES "Countries" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Addresses_Districts_DistrictId" FOREIGN KEY ("DistrictId") REFERENCES "Districts" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Addresses_Provinces_ProvinceId" FOREIGN KEY ("ProvinceId") REFERENCES "Provinces" ("Id") ON DELETE CASCADE,
        CONSTRAINT "FK_Addresses_Tehsils_CityId" FOREIGN KEY ("CityId") REFERENCES "Tehsils" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Addresses_CityId" ON "Addresses" ("CityId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Addresses_CountryId" ON "Addresses" ("CountryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Addresses_DistrictId" ON "Addresses" ("DistrictId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Addresses_ProvinceId" ON "Addresses" ("ProvinceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantAchievements_ApplicantId" ON "ApplicantAchievements" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantCertifications_ApplicantId" ON "ApplicantCertifications" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantDocuments_ApplicantId" ON "ApplicantDocuments" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantDocuments_ApplicantId1" ON "ApplicantDocuments" ("ApplicantId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantEducations_ApplicantId" ON "ApplicantEducations" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantEducations_ApplicantId1" ON "ApplicantEducations" ("ApplicantId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantEducations_DegreeLevelId" ON "ApplicantEducations" ("DegreeLevelId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantExperiences_ApplicantId" ON "ApplicantExperiences" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantExperiences_ApplicantId1" ON "ApplicantExperiences" ("ApplicantId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "IX_ApplicantFamilySummaries_ApplicantId" ON "ApplicantFamilySummaries" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "IX_ApplicantFinancialDetails_ApplicantId" ON "ApplicantFinancialDetails" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantInternalRelatives_ApplicantId" ON "ApplicantInternalRelatives" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantInternalRelatives_ApplicantId1" ON "ApplicantInternalRelatives" ("ApplicantId1");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "IX_ApplicantMilitaryDetails_ApplicantId" ON "ApplicantMilitaryDetails" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "IX_ApplicantPersonalInfos_ApplicantId" ON "ApplicantPersonalInfos" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "IX_Applicants_CNICNumber" ON "Applicants" ("CNICNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "IX_Applicants_TrackingCode" ON "Applicants" ("TrackingCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantSiblings_ApplicantId" ON "ApplicantSiblings" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicantSkills_ApplicantId" ON "ApplicantSkills" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicationStatusHistory_ChangedByUserId" ON "ApplicationStatusHistory" ("ChangedByUserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_ApplicationStatusHistory_JobApplicationId" ON "ApplicationStatusHistory" ("JobApplicationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_AspNetRoleClaims_RoleId" ON "AspNetRoleClaims" ("RoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_AspNetUserClaims_UserId" ON "AspNetUserClaims" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_AspNetUserLogins_UserId" ON "AspNetUserLogins" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_AspNetUserRoles_RoleId" ON "AspNetUserRoles" ("RoleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Districts_ProvinceId" ON "Districts" ("ProvinceId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_JobApplications_ApplicantId" ON "JobApplications" ("ApplicantId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_JobApplications_JobOpeningId" ON "JobApplications" ("JobOpeningId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_JobOpenings_CampaignId" ON "JobOpenings" ("CampaignId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_JobOpenings_ExpiresAt" ON "JobOpenings" ("ExpiresAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_JobOpenings_PostedAt" ON "JobOpenings" ("PostedAt");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_JobOpenings_Status" ON "JobOpenings" ("Status");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_JobSkillRequirements_JobOpeningId" ON "JobSkillRequirements" ("JobOpeningId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "IX_Permissions_Name" ON "Permissions" ("Name");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Provinces_CountryId" ON "Provinces" ("CountryId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "IX_RecruitmentCampaigns_CampaignCode" ON "RecruitmentCampaigns" ("CampaignCode");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_RolePermissions_PermissionId" ON "RolePermissions" ("PermissionId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON "Roles" ("NormalizedName") WHERE [NormalizedName] IS NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Tehsils_DistrictId" ON "Tehsils" ("DistrictId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "EmailIndex" ON "Users" ("NormalizedEmail");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Users_DepartmentId" ON "Users" ("DepartmentId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Users_DesignationId" ON "Users" ("DesignationId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE INDEX "IX_Users_PayScaleId" ON "Users" ("PayScaleId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON "Users" ("NormalizedUserName") WHERE [NormalizedUserName] IS NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421074347_Addres') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260421074347_Addres', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantAchievements" DROP CONSTRAINT "FK_ApplicantAchievements_Applicants_ApplicantId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantCertifications" DROP CONSTRAINT "FK_ApplicantCertifications_Applicants_ApplicantId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantDocuments" DROP CONSTRAINT "FK_ApplicantDocuments_Applicants_ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantEducations" DROP CONSTRAINT "FK_ApplicantEducations_Applicants_ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantExperiences" DROP CONSTRAINT "FK_ApplicantExperiences_Applicants_ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantInternalRelatives" DROP CONSTRAINT "FK_ApplicantInternalRelatives_Applicants_ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantSkills" DROP CONSTRAINT "FK_ApplicantSkills_Applicants_ApplicantId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    DROP INDEX "IX_ApplicantPersonalInfos_ApplicantId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    DROP INDEX "IX_ApplicantInternalRelatives_ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    DROP INDEX "IX_ApplicantExperiences_ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    DROP INDEX "IX_ApplicantEducations_ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    DROP INDEX "IX_ApplicantDocuments_ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "JobOpenings" DROP COLUMN "Department";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantInternalRelatives" DROP COLUMN "ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantExperiences" DROP COLUMN "ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantEducations" DROP COLUMN "ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantDocuments" DROP COLUMN "ApplicantId1";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantPersonalInfos" ALTER COLUMN "ApplicantId" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    UPDATE "ApplicantInternalRelatives" SET "ApplicantId" = '00000000-0000-0000-0000-000000000000' WHERE "ApplicantId" IS NULL;
    ALTER TABLE "ApplicantInternalRelatives" ALTER COLUMN "ApplicantId" SET NOT NULL;
    ALTER TABLE "ApplicantInternalRelatives" ALTER COLUMN "ApplicantId" SET DEFAULT '00000000-0000-0000-0000-000000000000';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantEducations" ALTER COLUMN "DegreeLevelId" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantDocuments" ALTER COLUMN "ApplicantId" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    CREATE UNIQUE INDEX "IX_ApplicantPersonalInfos_ApplicantId" ON "ApplicantPersonalInfos" ("ApplicantId") WHERE [ApplicantId] IS NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantAchievements" ADD CONSTRAINT "FK_ApplicantAchievements_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantCertifications" ADD CONSTRAINT "FK_ApplicantCertifications_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    ALTER TABLE "ApplicantSkills" ADD CONSTRAINT "FK_ApplicantSkills_Applicants_ApplicantId" FOREIGN KEY ("ApplicantId") REFERENCES "Applicants" ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421085252_Ad0dres') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260421085252_Ad0dres', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421101234_Added_columns') THEN
    ALTER TABLE "JobOpenings" ADD "Benefits" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421101234_Added_columns') THEN
    ALTER TABLE "JobOpenings" ADD "EmploymentType" nvarchar(max) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421101234_Added_columns') THEN
    ALTER TABLE "JobOpenings" ADD "IsFeatured" bit NOT NULL DEFAULT FALSE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421101234_Added_columns') THEN
    ALTER TABLE "JobOpenings" ADD "JobCategory" nvarchar(max) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421101234_Added_columns') THEN
    ALTER TABLE "JobOpenings" ADD "JobSlug" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421101234_Added_columns') THEN
    ALTER TABLE "JobOpenings" ADD "SalaryGrade" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421101234_Added_columns') THEN
    ALTER TABLE "JobOpenings" ADD "TotalPositions" int NOT NULL DEFAULT 0;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421101234_Added_columns') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260421101234_Added_columns', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421113421_Added_columns_appliedAt') THEN
    ALTER TABLE "JobOpenings" ALTER COLUMN "TotalPositions" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421113421_Added_columns_appliedAt') THEN
    ALTER TABLE "JobOpenings" ALTER COLUMN "JobCategory" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421113421_Added_columns_appliedAt') THEN
    ALTER TABLE "JobOpenings" ALTER COLUMN "EmploymentType" DROP NOT NULL;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421113421_Added_columns_appliedAt') THEN
    ALTER TABLE "JobApplications" ADD "AppliedAt" datetimeoffset NOT NULL DEFAULT TIMESTAMPTZ '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421113421_Added_columns_appliedAt') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260421113421_Added_columns_appliedAt', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421123859_Added_cnic_rm') THEN
    DROP INDEX "IX_Applicants_CNICNumber";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421123859_Added_cnic_rm') THEN
    ALTER TABLE "Applicants" ADD "UpdatedAt" datetime2 NOT NULL DEFAULT TIMESTAMPTZ '-infinity';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421123859_Added_cnic_rm') THEN
    CREATE INDEX "IX_Applicants_CNICNumber" ON "Applicants" ("CNICNumber");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421123859_Added_cnic_rm') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260421123859_Added_cnic_rm', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421151039_Added_cnic_geneder') THEN
    ALTER TABLE "ApplicantPersonalInfos" ADD "Gender" nvarchar(max) NOT NULL DEFAULT '';
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421151039_Added_cnic_geneder') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260421151039_Added_cnic_geneder', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421160632_Added_inv') THEN
    ALTER TABLE "Applicants" ALTER COLUMN "TrackingCode" TYPE nvarchar(50);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260421160632_Added_inv') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260421160632_Added_inv', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260422034727_data_fixed') THEN
    ALTER TABLE "ApplicantSiblings" ADD "Designation" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260422034727_data_fixed') THEN
    ALTER TABLE "ApplicantSiblings" ADD "Gender" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260422034727_data_fixed') THEN
    ALTER TABLE "ApplicantSiblings" ADD "MaritalStatus" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260422034727_data_fixed') THEN
    ALTER TABLE "ApplicantPersonalInfos" ADD "Accommodation" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260422034727_data_fixed') THEN
    ALTER TABLE "ApplicantFinancialDetails" ADD "OtherFacilities" nvarchar(max);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260422034727_data_fixed') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260422034727_data_fixed', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260422050647_Added_colv') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260422050647_Added_colv', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260423070247_FinalArchitecture') THEN
    CREATE TABLE "CampaignExportTasks" (
        "Id" uniqueidentifier NOT NULL,
        "CampaignId" uniqueidentifier NOT NULL,
        "RequestedByUserId" uniqueidentifier NOT NULL,
        "Status" nvarchar(max) NOT NULL,
        "DownloadUrl" nvarchar(max),
        "ErrorMessage" nvarchar(max),
        "ProcessedAt" datetime2,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_CampaignExportTasks" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_CampaignExportTasks_RecruitmentCampaigns_CampaignId" FOREIGN KEY ("CampaignId") REFERENCES "RecruitmentCampaigns" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260423070247_FinalArchitecture') THEN
    CREATE INDEX "IX_CampaignExportTasks_CampaignId" ON "CampaignExportTasks" ("CampaignId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260423070247_FinalArchitecture') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260423070247_FinalArchitecture', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260423104654_AddApplicationsNavigation') THEN
    ALTER TABLE "JobApplications" DROP CONSTRAINT "FK_JobApplications_JobOpenings_JobOpeningId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260423104654_AddApplicationsNavigation') THEN
    ALTER TABLE "JobApplications" ADD CONSTRAINT "FK_JobApplications_JobOpenings_JobOpeningId" FOREIGN KEY ("JobOpeningId") REFERENCES "JobOpenings" ("Id") ON DELETE RESTRICT;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260423104654_AddApplicationsNavigation') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260423104654_AddApplicationsNavigation', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424053523_Added_notication') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260424053523_Added_notication', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064345_Added_audit_notication') THEN
    CREATE TABLE "auditLogs" (
        "Id" uniqueidentifier NOT NULL,
        "UserId" nvarchar(max),
        "UserName" nvarchar(max),
        "Action" nvarchar(max) NOT NULL,
        "EntityName" nvarchar(max) NOT NULL,
        "EntityId" nvarchar(max),
        "IPAddress" nvarchar(max),
        "UserAgent" nvarchar(max),
        "Path" nvarchar(max),
        "OldValues" nvarchar(max),
        "NewValues" nvarchar(max),
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_auditLogs" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064345_Added_audit_notication') THEN
    CREATE TABLE "userNotifications" (
        "Id" uniqueidentifier NOT NULL,
        "UserId" uniqueidentifier NOT NULL,
        "Title" nvarchar(max) NOT NULL,
        "Message" nvarchar(max) NOT NULL,
        "ActionUrl" nvarchar(max),
        "Type" nvarchar(max) NOT NULL,
        "IsRead" bit NOT NULL,
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_userNotifications" PRIMARY KEY ("Id"),
        CONSTRAINT "FK_userNotifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064345_Added_audit_notication') THEN
    CREATE INDEX "IX_userNotifications_UserId" ON "userNotifications" ("UserId");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064345_Added_audit_notication') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260424064345_Added_audit_notication', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER TABLE "userNotifications" DROP CONSTRAINT "FK_userNotifications_Users_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER TABLE "userNotifications" DROP CONSTRAINT "PK_userNotifications";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER TABLE "auditLogs" DROP CONSTRAINT "PK_auditLogs";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER TABLE "userNotifications" RENAME TO "UserNotifications";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER TABLE "auditLogs" RENAME TO "AuditLogs";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER INDEX "IX_userNotifications_UserId" RENAME TO "IX_UserNotifications_UserId";
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER TABLE "UserNotifications" ADD CONSTRAINT "PK_UserNotifications" PRIMARY KEY ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER TABLE "AuditLogs" ADD CONSTRAINT "PK_AuditLogs" PRIMARY KEY ("Id");
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    ALTER TABLE "UserNotifications" ADD CONSTRAINT "FK_UserNotifications_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE;
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424064730_Added_audit_notication_correction') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260424064730_Added_audit_notication_correction', '10.0.7');
    END IF;
END $EF$;
COMMIT;

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424144112_Added_support_ticket_correction') THEN
    CREATE TABLE "SupportTickets" (
        "Id" uniqueidentifier NOT NULL,
        "Email" nvarchar(max) NOT NULL,
        "Message" nvarchar(max) NOT NULL,
        "Status" nvarchar(max) NOT NULL,
        "IPAddress" nvarchar(max),
        "UserAgent" nvarchar(max),
        "CreatedAt" datetimeoffset NOT NULL,
        "IsDeleted" bit NOT NULL,
        "DeletedAt" datetimeoffset,
        CONSTRAINT "PK_SupportTickets" PRIMARY KEY ("Id")
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260424144112_Added_support_ticket_correction') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260424144112_Added_support_ticket_correction', '10.0.7');
    END IF;
END $EF$;
COMMIT;

