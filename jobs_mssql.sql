/* 
   CAREER 635 - PERSONNEL DIVISION MASTER DATA REQUISITION
   CONSOLIDATED REQUISITION SCRIPT: PAGES 1, 2, & 3
   VALIDITY: 2026-04-26 TO 2026-05-15
*/

-- 1. CLEAN START: Remove all existing records to prevent key conflicts
DELETE FROM [JobSkillRequirements];
DELETE FROM [JobApplications];
DELETE FROM [JobOpenings];
DELETE FROM [RecruitmentCampaigns];

-- 2. SYSTEM VARIABLES
DECLARE @CampaignId UNIQUEIDENTIFIER = NEWID();
DECLARE @StartDate DATETIME = '2026-04-26 09:00:00';
DECLARE @EndDate DATETIME = '2026-05-15 23:59:59';
DECLARE @Now DATETIME = GETUTCDATE();

-- 3. ESTABLISH THE RECRUITMENT CAMPAIGN
INSERT INTO [RecruitmentCampaigns] ([Id], [Name], [CampaignCode], [IsActive], [IsDeleted], [CreatedAt])
VALUES (@CampaignId, N'ICT Equipment & Manufacturing Solutions Requisition 2026', N'NRTC-ICT-2026', 1, 0, @Now);

-- 4. INSERT ALL JOB REQUISITIONS
INSERT INTO [JobOpenings] 
(
    [Id], [CampaignId], [Title], [JobCategory], [EmploymentType], [TotalPositions], 
    [Description], [Requirements], [MinEducationLevel], [RequiredMajorField], 
    [RequiredExperienceYears], [IsPecRequired], [Status], [PostedAt], [ExpiresAt], 
    [CreatedAt], [IsDeleted]
)
VALUES
-- PAGE 1
(NEWID(), @CampaignId, N'Manager', N'Technical', NULL, 5, 
N'**Overview:** Strategic and technical oversight of ICT equipment manufacturing and system solutions.', 
N'- **Qualification:** Master / Bachelors in Engineering or equivalent.
- **Eligibility:** Minimum 04-06 years of technical experience in relevant field.', N'Bachelors', N'Engineering', 4.0, 1, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Assistant Executive Engineer', N'Engineering', NULL, 8, 
N'**Overview:** Engineering support for Electrical, Electronic, Telecom, Mechatronic, Mechanical, and Industrial disciplines.', 
N'- **Qualification:** MS / BE Electrical, Electronic, Telecom, Mechatronic, Mechanical, Industrial.
- **Eligibility:** Minimum 02-05 years of experience. RF, Microwave, or relevant experience is preferable.', N'Bachelors', N'Engineering', 2.0, 1, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Assistant Manager', N'Technical', NULL, 3, 
N'**Overview:** Support and management of CS, InfoSec, AI, Cybersecurity, and Forensic projects.', 
N'- **Qualification:** MS/ BS /BE Computer Science, Information Security, Software, AI, Cyber security, Forensic.
- **Eligibility:** Minimum 02-05 years of post-qualified experience is preferable.', N'Bachelors', N'CS/IT/Security', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'AI Senior Developer', N'Technical', NULL, 7, 
N'**Overview:** Lead developer for AI Development and Design related technical implementations.', 
N'- **Qualification:** PHD /M. Phil /MS in Computer Science / Computer Software/ Computer Engineering.
- **Eligibility:** Minimum 04-06 years of experience in AI Development and Design related work.', N'Masters', N'AI/CS', 4.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Senior Software Developer', N'Technical', NULL, 16, 
N'**Overview:** Specialized development in Desktop/Web, Microservices, and Native Mobile applications.', 
N'- **Qualification:** BSCS / MSCS / IT / BE Computer or equivalent.
- **Eligibility:** Minimum 04 years of experience in app development, hybrid/native mobile apps, and machine learning.', N'Bachelors', N'CS/IT', 4.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Cyber Security Engineer', N'Technical', NULL, 5, 
N'**Overview:** Implementation of cryptographic standards, algorithms, and secure protocol solutions.', 
N'- **Qualification:** MS in Information Security / Cyber Security OR BS / BE in Electrical / Computer / Telecom / Software.
- **Eligibility:** Minimum 01 year of experience in understanding of crypto algorithms, cryptographic standards and protocols.', N'Bachelors', N'Cyber Security', 1.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Site Engineer', N'Engineering', NULL, 30, 
N'**Overview:** Field engineering and site coordination for Safe Cities and related infrastructure projects.', 
N'- **Qualification:** BE Electrical, Electronic, Telecom, Mechatronic, Mechanical, BS Software, Computer Science.
- **Eligibility:** Minimum 02-05 years of post-qualified experience. Field related and safe cities experience is preferable.', N'Bachelors', N'Engineering', 2.0, 1, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Manager (Operations)', N'Management', NULL, 7, 
N'**Overview:** Oversight of Sales & Marketing, General Administration, and Supply Chain cycles.', 
N'- **Qualification:** Master / Bachelors or equivalent in any discipline.
- **Eligibility:** Minimum 04-06 years of experience in Sales & Marketing, Admin and Supply chain.', N'Bachelors', N'Any Discipline', 4.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Assistant Manager (Operations)', N'Management', NULL, 4, 
N'**Overview:** Managing Finance, HR, or other relevant management disciplines.', 
N'- **Qualification:** MBA/BBA (Finance, HR) or relevant in any discipline.
- **Eligibility:** Minimum 02-05 years of post-qualified experience is preferable.', N'Bachelors', N'MBA/BBA', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Marketing Officer', N'Management', NULL, 6, 
N'**Overview:** Coordination and execution of marketing and outreach programs.', 
N'- **Qualification:** MBA Marketing.
- **Eligibility:** Minimum 02-04 years of post-qualification experience is preferable.', N'Bachelors', N'Marketing', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Procurement Officer', N'Management', NULL, 3, 
N'**Overview:** Managing the procurement lifecycle and SCM operations.', 
N'- **Qualification:** MS/MBA Supply Chain Mgt.
- **Eligibility:** Minimum 02-04 years of post-qualification experience. SCM related experience is preferable.', N'Masters', N'SCM/MBA', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Graphic Designer', N'Creative', NULL, 3, 
N'**Overview:** Multimedia animations, logo design, banners, brochures, and shields.', 
N'- **Qualification:** MS/BS in Relevant Field.
- **Eligibility:** Minimum 05 years of experience in understanding of Graphic Design. Skills: Adobe Photoshop, Corel Draw, Blender, Adobe Flash.', N'Bachelors', N'Graphics/Design', 5.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

-- PAGE 2
(NEWID(), @CampaignId, N'Photographer', N'Creative', NULL, 2, 
N'**Overview:** High-end professional corporate photography.', 
N'- **Qualification:** BS in Relevant Field.
- **Eligibility:** Minimum 05 Years experience at Corporate level. Relevant certificates preferable.', N'Bachelors', N'Photography', 5.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Videographer', N'Creative', NULL, 1, 
N'**Overview:** Professional videography and post-production.', 
N'- **Qualification:** BS in Relevant Field.
- **Eligibility:** Minimum 05 Years experience in videography. Relevant certificates preferable.', N'Bachelors', N'Videography', 5.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Admin Officer (Army)', N'Management', NULL, 2, 
N'**Overview:** Administrative oversight and internal disciplinary coordination.', 
N'- **Qualification:** Master / Bachelor in any discipline.
- **Eligibility:** Ex-Army person preferable.', N'Bachelors', N'Any Discipline', 0.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Admin Officer', N'Management', NULL, 1, 
N'**Overview:** General office management and administration.', 
N'- **Qualification:** MA/BS in any discipline.
- **Eligibility:** Minimum 02-03 Years relevant experience is preferable.', N'Bachelors', N'Any Discipline', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Auditor (Registry-1)', N'Finance', NULL, 3, 
N'**Overview:** Internal audit, taxation compliance, and financial verification.', 
N'- **Qualification:** MBA /BBA (Finance & Banking) or relevant.
- **Eligibility:** Minimum 04-06 years of post-relevant experience. Audit/MS Office cert preferable.', N'Bachelors', N'Finance/Banking', 4.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Auditor (Registry-2)', N'Finance', NULL, 3, 
N'**Overview:** Financial audit and accounts verification.', 
N'- **Qualification:** MBA /BBA (Finance & Banking) or relevant.
- **Eligibility:** Minimum 04-06 years of post-relevant experience.', N'Bachelors', N'Finance/Banking', 4.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Accounts Officer', N'Finance', NULL, 4, 
N'**Overview:** Organizational accounting and ERP software operations.', 
N'- **Qualification:** MBA /BBA (Finance & Banking) or relevant.
- **Eligibility:** Minimum 04-06 years of post-qualification experience. ERP/MS Office cert preferable.', N'Bachelors', N'Finance/Banking', 4.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Accountant', N'Finance', NULL, 3, 
N'**Overview:** Daily maintenance of ledgers and financial reporting.', 
N'- **Qualification:** MBA /BBA (Finance & Banking) or relevant.
- **Eligibility:** Minimum 02-04 years of post-qualification experience is preferable.', N'Bachelors', N'Finance/Banking', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Junior Officer', N'Management', NULL, 5, 
N'**Overview:** Management support and general administrative tasking.', 
N'- **Qualification:** Masters or equivalent in any discipline.
- **Eligibility:** Minimum 04-06 Years relevant field experience.', N'Masters', N'Any Discipline', 4.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Sr. Chargeman', N'Technical', NULL, 10, 
N'**Overview:** Technical workshop and manufacturing line supervision.', 
N'- **Qualification:** B. Tech in any discipline.
- **Eligibility:** Minimum 05-06 Years relevant experience. Ex-Army / Air Force person Preferable.', N'Bachelors', N'B.Tech', 5.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Chargeman', N'Technical', NULL, 15, 
N'**Overview:** Technical support for Electrical, Mechanical, Telecom, or IT units.', 
N'- **Qualification:** DAE Electrical, Electronic, Mechanical, Mechatronic, Telecom, Civil, IT & Computer.
- **Eligibility:** Minimum 02-04 Years relevant experience. Technical Certification is preferable.', N'Intermediate', N'DAE', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Site Supervisor', N'Technical', NULL, 30, 
N'**Overview:** Technical site installation and field operations.', 
N'- **Qualification:** B.Tech Electrical, Electronic, IT, Computer, Mechanical.
- **Eligibility:** Minimum 01-02 years of experience is preferable.', N'Bachelors', N'B.Tech', 1.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Senior Assistant', N'Admin', NULL, 1, 
N'**Overview:** High-level administrative and clerical support.', 
N'- **Qualification:** MBA /M. Com or relevant.
- **Eligibility:** Minimum 04-06 Years relevant field experience.', N'Masters', N'MBA/M.Com', 4.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Office Assistant', N'Admin', NULL, 7, 
N'**Overview:** Data entry, documentation, and office hospitality.', 
N'- **Qualification:** MA/BA or equivalent in any discipline.
- **Eligibility:** Minimum 02-03 Years relevant experience. Typing Speed 50-60 WPM.', N'Bachelors', N'Any Discipline', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

-- PAGE 3
(NEWID(), @CampaignId, N'Beautician (Female)', N'Skilled', NULL, 2, 
N'**Overview:** Professional beauty and grooming services.', 
N'- **Qualification:** Matric/FA/BA in any discipline/Diploma.
- **Eligibility:** Minimum 02-03 Years relevant experience.', N'Matric', N'Diploma', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Embroidery Teacher (Female)', N'Skilled', NULL, 2, 
N'**Overview:** Instruction in stitching and embroidery.', 
N'- **Qualification:** Matric/FA/BA in any discipline/Diploma.
- **Eligibility:** Minimum 02-03 Years relevant experience.', N'Matric', N'Diploma', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'UDC', N'Admin', NULL, 8, 
N'**Overview:** Upper Division Clerk responsibilities.', 
N'- **Qualification:** F.A / F.Sc or Equivalent.
- **Eligibility:** Minimum 02-03 Years experience. Typing Speed 30-40 WPM preferable.', N'Intermediate', N'FA/FSc', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'LDC/DEO', N'Admin', NULL, 3, 
N'**Overview:** Lower Division Clerk and Data Entry Operator.', 
N'- **Qualification:** Matric or Equivalent.
- **Eligibility:** Minimum 02-03 Years experience. Typing Speed 30 WPM preferable.', N'Matric', N'Any Discipline', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Splicer / Technician', N'Technical', NULL, 15, 
N'**Overview:** Splicing and maintenance of technical equipment.', 
N'- **Qualification:** DAE Electrical, Electronic, Mechatronic, Telecom, Mechanical, Civil.
- **Eligibility:** Minimum 01-02 years of post-qualification experience is preferable.', N'Intermediate', N'DAE', 1.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Operator', N'Technical', NULL, 7, 
N'**Overview:** Equipment handling and machinery operation.', 
N'- **Qualification:** F.A / F.Sc or Equivalent.
- **Eligibility:** Minimum 02-03 Years relevant experience is preferable.', N'Intermediate', N'FA/FSc', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Production Worker', N'Skilled', NULL, 7, 
N'**Overview:** Support for manufacturing and production units.', 
N'- **Qualification:** Matric / Apprenticeship.
- **Eligibility:** Minimum 02-04 years technical experience is preferable.', N'Matric', N'Technical', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'General Worker', N'Labor', NULL, 12, 
N'**Overview:** General tasking across all departments.', 
N'- **Qualification:** Under Matric / Matric.
- **Eligibility:** Experience in any field.', N'Primary', N'Any', 0.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Cook / Waiter', N'Skilled', NULL, 5, 
N'**Overview:** Kitchen management and mess services.', 
N'- **Qualification:** Nil / Under Matric / Matric.
- **Eligibility:** Minimum 02-04 years experience. Ex-Army person preferable.', N'Primary', N'Any', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Mali', N'Labor', NULL, 4, 
N'**Overview:** Horticultural and landscape maintenance.', 
N'- **Qualification:** Nil / Under Matric / Matric.
- **Eligibility:** Minimum 02-03 Years experience preferable.', N'Primary', N'Any', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Ayya / Attendant (Female)', N'Labor', NULL, 3, 
N'**Overview:** Supporting services (Female only).', 
N'- **Qualification:** Nil / Primary.
- **Eligibility:** Minimum 02-03 Years experience preferable.', N'Primary', N'Any', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Office Boy', N'Admin', NULL, 4, 
N'**Overview:** Message delivery and administrative support.', 
N'- **Qualification:** Under Matric / Matric.
- **Eligibility:** Minimum 02-03 Years experience preferable.', N'Primary', N'Matric', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Security Guard', N'Security', NULL, 2, 
N'**Overview:** Surveillance and physical security.', 
N'- **Qualification:** BA / FA / Matric.
- **Eligibility:** Minimum 02-04 years post-qualification experience. Ex-Army person preferable.', N'Matric', N'BA/FA/Matric', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Driver', N'Skilled', NULL, 5, 
N'**Overview:** Transport of personnel and organizational fleet.', 
N'- **Qualification:** BA / FA / Matric.
- **Eligibility:** Minimum 02-04 years post-qualification experience. Ex-Army person preferable.', N'Matric', N'BA/FA/Matric', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Sanitation Worker', N'Labor', NULL, 3, 
N'**Overview:** General cleaning and sanitation of facilities.', 
N'- **Qualification:** Nil / Primary.
- **Eligibility:** Minimum 02-03 Years experience preferable.', N'Primary', N'Any', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Dishwasher', N'Labor', NULL, 2, 
N'**Overview:** Cleaning of kitchen utensils and equipment.', 
N'- **Qualification:** Nil / Primary.
- **Eligibility:** Minimum 02-03 Years experience preferable.', N'Primary', N'Any', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0),

(NEWID(), @CampaignId, N'Car Washer', N'Labor', NULL, 2, 
N'**Overview:** Maintenance and washing of fleet vehicles.', 
N'- **Qualification:** Nil / Primary.
- **Eligibility:** Minimum 02-03 Years experience preferable.', N'Primary', N'Any', 2.0, 0, N'Published', @StartDate, @EndDate, @Now, 0);

PRINT 'SUCCESS: 43 Requisitions deployed for the 2026 Recruitment Cycle.';