/* 
   CAREER 635 - PERSONNEL DIVISION MASTER DATA REQUISITION
   CONSOLIDATED REQUISITION SCRIPT: PAGES 1, 2, & 3
   VALIDITY: 2026-04-26 TO 2026-05-15
*/

-- 1. CLEAN START: Remove all existing records
TRUNCATE TABLE job_skill_requirements CASCADE;
TRUNCATE TABLE job_applications CASCADE;
TRUNCATE TABLE job_openings CASCADE;
TRUNCATE TABLE recruitment_campaigns CASCADE;

DO $$ 
DECLARE 
    v_camp_id UUID := gen_random_uuid();
    v_start TIMESTAMP := '2026-04-26 09:00:00';
    v_end   TIMESTAMP := '2026-05-15 23:59:59';
    v_now   TIMESTAMP := NOW();
BEGIN

-- 2. ESTABLISH THE RECRUITMENT CAMPAIGN
INSERT INTO recruitment_campaigns (id, name, campaign_code, is_active, is_deleted, created_at)
VALUES (v_camp_id, 'ICT Equipment & Manufacturing Solutions Requisition 2026', 'NRTC-ICT-2026', TRUE, FALSE, v_now);

-- 3. INSERT ALL JOB REQUISITIONS
INSERT INTO job_openings 
(
    id, campaign_id, title, job_category, employment_type, total_positions, 
    description, requirements, min_education_level, required_major_field, 
    required_experience_years, is_pec_required, status, posted_at, expires_at, 
    created_at, is_deleted
)
VALUES
-- PAGE 1
(gen_random_uuid(), v_camp_id, 'Manager', 'Technical', NULL, 5, 
'**Overview:** Strategic and technical oversight of ICT equipment manufacturing and system solutions.', 
'- **Qualification:** Master / Bachelors in Engineering or equivalent.
- **Eligibility:** Minimum 04-06 years of technical experience in relevant field.', 'Bachelors', 'Engineering', 4.0, TRUE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Assistant Executive Engineer', 'Engineering', NULL, 8, 
'**Overview:** Engineering support for Electrical, Electronic, Telecom, Mechatronic, Mechanical, and Industrial disciplines.', 
'- **Qualification:** MS / BE Electrical, Electronic, Telecom, Mechatronic, Mechanical, Industrial.
- **Eligibility:** Minimum 02-05 years of experience. RF, Microwave, or relevant experience is preferable.', 'Bachelors', 'Engineering', 2.0, TRUE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Assistant Manager', 'Technical', NULL, 3, 
'**Overview:** Support and management of CS, InfoSec, AI, Cybersecurity, and Forensic projects.', 
'- **Qualification:** MS/ BS /BE Computer Science, Information Security, Software, AI, Cyber security, Forensic.
- **Eligibility:** Minimum 02-05 years of post-qualified experience is preferable.', 'Bachelors', 'CS/IT/Security', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'AI Senior Developer', 'Technical', NULL, 7, 
'**Overview:** Lead developer for AI Development and Design related technical implementations.', 
'- **Qualification:** PHD /M. Phil /MS in Computer Science / Computer Software/ Computer Engineering.
- **Eligibility:** Minimum 04-06 years of experience in AI Development and Design related work.', 'Masters', 'AI/CS', 4.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Senior Software Developer', 'Technical', NULL, 16, 
'**Overview:** Specialized development in Desktop/Web, Microservices, and Native Mobile applications.', 
'- **Qualification:** BSCS / MSCS / IT / BE Computer or equivalent.
- **Eligibility:** Minimum 04 years of experience in app development, hybrid/native mobile apps, and machine learning.', 'Bachelors', 'CS/IT', 4.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Cyber Security Engineer', 'Technical', NULL, 5, 
'**Overview:** Implementation of cryptographic standards, algorithms, and secure protocol solutions.', 
'- **Qualification:** MS in Information Security / Cyber Security OR BS / BE in Electrical / Computer / Telecom / Software.
- **Eligibility:** Minimum 01 year of experience in understanding of crypto algorithms, cryptographic standards and protocols.', 'Bachelors', 'Cyber Security', 1.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Site Engineer', 'Engineering', NULL, 30, 
'**Overview:** Field engineering and site coordination for Safe Cities and related infrastructure projects.', 
'- **Qualification:** BE Electrical, Electronic, Telecom, Mechatronic, Mechanical, BS Software, Computer Science.
- **Eligibility:** Minimum 02-05 years of post-qualified experience. Field related and safe cities experience is preferable.', 'Bachelors', 'Engineering', 2.0, TRUE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Manager (Operations)', 'Management', NULL, 7, 
'**Overview:** Oversight of Sales & Marketing, General Administration, and Supply Chain cycles.', 
'- **Qualification:** Master / Bachelors or equivalent in any discipline.
- **Eligibility:** Minimum 04-06 years of experience in Sales & Marketing, Admin and Supply chain.', 'Bachelors', 'Any Discipline', 4.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Assistant Manager (Operations)', 'Management', NULL, 4, 
'**Overview:** Managing Finance, HR, or other relevant management disciplines.', 
'- **Qualification:** MBA/BBA (Finance, HR) or relevant in any discipline.
- **Eligibility:** Minimum 02-05 years of post-qualified experience is preferable.', 'Bachelors', 'MBA/BBA', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Marketing Officer', 'Management', NULL, 6, 
'**Overview:** Coordination and execution of marketing and outreach programs.', 
'- **Qualification:** MBA Marketing.
- **Eligibility:** Minimum 02-04 years of post-qualification experience is preferable.', 'Bachelors', 'Marketing', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Procurement Officer', 'Management', NULL, 3, 
'**Overview:** Managing the procurement lifecycle and SCM operations.', 
'- **Qualification:** MS/MBA Supply Chain Mgt.
- **Eligibility:** Minimum 02-04 years of post-qualification experience. SCM related experience is preferable.', 'Masters', 'SCM/MBA', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Graphic Designer', 'Creative', NULL, 3, 
'**Overview:** Multimedia animations, logo design, banners, brochures, and shields.', 
'- **Qualification:** MS/BS in Relevant Field.
- **Eligibility:** Minimum 05 years of experience in understanding of Graphic Design. Skills: Adobe Photoshop, Corel Draw, Blender, Adobe Flash.', 'Bachelors', 'Graphics/Design', 5.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

-- PAGE 2
(gen_random_uuid(), v_camp_id, 'Photographer', 'Creative', NULL, 2, 
'**Overview:** High-end professional corporate photography.', 
'- **Qualification:** BS in Relevant Field.
- **Eligibility:** Minimum 05 Years experience at Corporate level. Relevant certificates preferable.', 'Bachelors', 'Photography', 5.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Videographer', 'Creative', NULL, 1, 
'**Overview:** Professional videography and post-production.', 
'- **Qualification:** BS in Relevant Field.
- **Eligibility:** Minimum 05 Years experience in videography. Relevant certificates preferable.', 'Bachelors', 'Videography', 5.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Admin Officer (Army)', 'Management', NULL, 2, 
'**Overview:** Administrative oversight and internal disciplinary coordination.', 
'- **Qualification:** Master / Bachelor in any discipline.
- **Eligibility:** Ex-Army person preferable.', 'Bachelors', 'Any Discipline', 0.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Admin Officer', 'Management', NULL, 1, 
'**Overview:** General office management and administration.', 
'- **Qualification:** MA/BS in any discipline.
- **Eligibility:** Minimum 02-03 Years relevant experience is preferable.', 'Bachelors', 'Any Discipline', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Auditor (Registry-1)', 'Finance', NULL, 3, 
'**Overview:** Internal audit, taxation compliance, and financial verification.', 
'- **Qualification:** MBA /BBA (Finance & Banking) or relevant.
- **Eligibility:** Minimum 04-06 years of post-relevant experience. Audit/MS Office cert preferable.', 'Bachelors', 'Finance/Banking', 4.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Auditor (Registry-2)', 'Finance', NULL, 3, 
'**Overview:** Financial audit and accounts verification.', 
'- **Qualification:** MBA /BBA (Finance & Banking) or relevant.
- **Eligibility:** Minimum 04-06 years of post-relevant experience.', 'Bachelors', 'Finance/Banking', 4.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Accounts Officer', 'Finance', NULL, 4, 
'**Overview:** Organizational accounting and ERP software operations.', 
'- **Qualification:** MBA /BBA (Finance & Banking) or relevant.
- **Eligibility:** Minimum 04-06 years of post-qualification experience. ERP/MS Office cert preferable.', 'Bachelors', 'Finance/Banking', 4.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Accountant', 'Finance', NULL, 3, 
'**Overview:** Daily maintenance of ledgers and financial reporting.', 
'- **Qualification:** MBA /BBA (Finance & Banking) or relevant.
- **Eligibility:** Minimum 02-04 years of post-qualification experience is preferable.', 'Bachelors', 'Finance/Banking', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Junior Officer', 'Management', NULL, 5, 
'**Overview:** Management support and general administrative tasking.', 
'- **Qualification:** Masters or equivalent in any discipline.
- **Eligibility:** Minimum 04-06 Years relevant field experience.', 'Masters', 'Any Discipline', 4.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Sr. Chargeman', 'Technical', NULL, 10, 
'**Overview:** Technical workshop and manufacturing line supervision.', 
'- **Qualification:** B. Tech in any discipline.
- **Eligibility:** Minimum 05-06 Years relevant experience. Ex-Army / Air Force person Preferable.', 'Bachelors', 'B.Tech', 5.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Chargeman', 'Technical', NULL, 15, 
'**Overview:** Technical support for Electrical, Mechanical, Telecom, or IT units.', 
'- **Qualification:** DAE Electrical, Electronic, Mechanical, Mechatronic, Telecom, Civil, IT & Computer.
- **Eligibility:** Minimum 02-04 Years relevant experience. Technical Certification is preferable.', 'Intermediate', 'DAE', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Site Supervisor', 'Technical', NULL, 30, 
'**Overview:** Technical site installation and field operations.', 
'- **Qualification:** B.Tech Electrical, Electronic, IT, Computer, Mechanical.
- **Eligibility:** Minimum 01-02 years of experience is preferable.', 'Bachelors', 'B.Tech', 1.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Senior Assistant', 'Admin', NULL, 1, 
'**Overview:** High-level administrative and clerical support.', 
'- **Qualification:** MBA /M. Com or relevant.
- **Eligibility:** Minimum 04-06 Years relevant field experience.', 'Masters', 'MBA/M.Com', 4.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Office Assistant', 'Admin', NULL, 7, 
'**Overview:** Data entry, documentation, and office hospitality.', 
'- **Qualification:** MA/BA or equivalent in any discipline.
- **Eligibility:** Minimum 02-03 Years relevant experience. Typing Speed 50-60 WPM.', 'Bachelors', 'Any Discipline', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

-- PAGE 3
(gen_random_uuid(), v_camp_id, 'Beautician (Female)', 'Skilled', NULL, 2, 
'**Overview:** Professional beauty and grooming services.', 
'- **Qualification:** Matric/FA/BA in any discipline/Diploma.
- **Eligibility:** Minimum 02-03 Years relevant experience.', 'Matric', 'Diploma', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Embroidery Teacher (Female)', 'Skilled', NULL, 2, 
'**Overview:** Instruction in stitching and embroidery.', 
'- **Qualification:** Matric/FA/BA in any discipline/Diploma.
- **Eligibility:** Minimum 02-03 Years relevant experience.', 'Matric', 'Diploma', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'UDC', 'Admin', NULL, 8, 
'**Overview:** Upper Division Clerk responsibilities.', 
'- **Qualification:** F.A / F.Sc or Equivalent.
- **Eligibility:** Minimum 02-03 Years experience. Typing Speed 30-40 WPM preferable.', 'Intermediate', 'FA/FSc', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'LDC/DEO', 'Admin', NULL, 3, 
'**Overview:** Lower Division Clerk and Data Entry Operator.', 
'- **Qualification:** Matric or Equivalent.
- **Eligibility:** Minimum 02-03 Years experience. Typing Speed 30 WPM preferable.', 'Matric', 'Any Discipline', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Splicer / Technician', 'Technical', NULL, 15, 
'**Overview:** Splicing and maintenance of technical equipment.', 
'- **Qualification:** DAE Electrical, Electronic, Mechatronic, Telecom, Mechanical, Civil.
- **Eligibility:** Minimum 01-02 years of post-qualification experience is preferable.', 'Intermediate', 'DAE', 1.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Operator', 'Technical', NULL, 7, 
'**Overview:** Equipment handling and machinery operation.', 
'- **Qualification:** F.A / F.Sc or Equivalent.
- **Eligibility:** Minimum 02-03 Years relevant experience is preferable.', 'Intermediate', 'FA/FSc', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Production Worker', 'Skilled', NULL, 7, 
'**Overview:** Support for manufacturing and production units.', 
'- **Qualification:** Matric / Apprenticeship.
- **Eligibility:** Minimum 02-04 years technical experience is preferable.', 'Matric', 'Technical', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'General Worker', 'Labor', NULL, 12, 
'**Overview:** General tasking across all departments.', 
'- **Qualification:** Under Matric / Matric.
- **Eligibility:** Experience in any field.', 'Primary', 'Any', 0.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Cook / Waiter', 'Skilled', NULL, 5, 
'**Overview:** Kitchen management and mess services.', 
'- **Qualification:** Nil / Under Matric / Matric.
- **Eligibility:** Minimum 02-04 years experience. Ex-Army person preferable.', 'Primary', 'Any', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Mali', 'Labor', NULL, 4, 
'**Overview:** Horticultural and landscape maintenance.', 
'- **Qualification:** Nil / Under Matric / Matric.
- **Eligibility:** Minimum 02-03 Years experience preferable.', 'Primary', 'Any', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Ayya / Attendant (Female)', 'Labor', NULL, 3, 
'**Overview:** Supporting services (Female only).', 
'- **Qualification:** Nil / Primary.
- **Eligibility:** Minimum 02-03 Years experience preferable.', 'Primary', 'Any', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Office Boy', 'Admin', NULL, 4, 
'**Overview:** Message delivery and administrative support.', 
'- **Qualification:** Under Matric / Matric.
- **Eligibility:** Minimum 02-03 Years experience preferable.', 'Primary', 'Matric', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Security Guard', 'Security', NULL, 2, 
'**Overview:** Surveillance and physical security.', 
'- **Qualification:** BA / FA / Matric.
- **Eligibility:** Minimum 02-04 years post-qualification experience. Ex-Army person preferable.', 'Matric', 'BA/FA/Matric', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Driver', 'Skilled', NULL, 5, 
'**Overview:** Transport of personnel and organizational fleet.', 
'- **Qualification:** BA / FA / Matric.
- **Eligibility:** Minimum 02-04 years post-qualification experience. Ex-Army person preferable.', 'Matric', 'BA/FA/Matric', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Sanitation Worker', 'Labor', NULL, 3, 
'**Overview:** General cleaning and sanitation of facilities.', 
'- **Qualification:** Nil / Primary.
- **Eligibility:** Minimum 02-03 Years experience preferable.', 'Primary', 'Any', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Dishwasher', 'Labor', NULL, 2, 
'**Overview:** Cleaning of kitchen utensils and equipment.', 
'- **Qualification:** Nil / Primary.
- **Eligibility:** Minimum 02-03 Years experience preferable.', 'Primary', 'Any', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE),

(gen_random_uuid(), v_camp_id, 'Car Washer', 'Labor', NULL, 2, 
'**Overview:** Maintenance and washing of fleet vehicles.', 
'- **Qualification:** Nil / Primary.
- **Eligibility:** Minimum 02-03 Years experience preferable.', 'Primary', 'Any', 2.0, FALSE, 'Published', v_start, v_end, v_now, FALSE);

END $$;