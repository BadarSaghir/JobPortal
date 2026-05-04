CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    migration_id character varying(150) NOT NULL,
    product_version character varying(32) NOT NULL,
    CONSTRAINT pk___ef_migrations_history PRIMARY KEY (migration_id)
);

START TRANSACTION;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicants (
        id uuid NOT NULL,
        full_name character varying(200) NOT NULL,
        cnic_number character varying(15) NOT NULL,
        passport_image_url character varying(500),
        cv_url character varying(500),
        tracking_code character varying(50) NOT NULL,
        applied_at timestamp with time zone NOT NULL,
        updated_at timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicants PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "AspNetRoles" (
        id uuid NOT NULL,
        description character varying(500),
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        name character varying(256),
        normalized_name character varying(256),
        concurrency_stamp text,
        CONSTRAINT pk_asp_net_roles PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE audit_logs (
        id uuid NOT NULL,
        user_id text,
        user_name text,
        action text NOT NULL,
        entity_name text NOT NULL,
        entity_id text,
        ip_address text,
        user_agent text,
        path text,
        old_values text,
        new_values text,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_audit_logs PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE countries (
        id uuid NOT NULL,
        name text NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_countries PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE degree_levels (
        id uuid NOT NULL,
        name text NOT NULL,
        level_order integer NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_degree_levels PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE departments (
        id uuid NOT NULL,
        name character varying(100) NOT NULL,
        code character varying(20),
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_departments PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE designations (
        id uuid NOT NULL,
        title character varying(100) NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_designations PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "PayScales" (
        id uuid NOT NULL,
        grade character varying(50) NOT NULL,
        description text,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_pay_scales PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE permissions (
        id uuid NOT NULL,
        name character varying(100) NOT NULL,
        display_name character varying(150) NOT NULL,
        module text NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_permissions PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE recruitment_campaigns (
        id uuid NOT NULL,
        name character varying(200) NOT NULL,
        campaign_code character varying(50) NOT NULL,
        is_active boolean NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_recruitment_campaigns PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE support_tickets (
        id uuid NOT NULL,
        email text NOT NULL,
        message text NOT NULL,
        status text NOT NULL,
        ip_address text,
        user_agent text,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_support_tickets PRIMARY KEY (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_achievements (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        title text NOT NULL,
        description text,
        date_received timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_achievements PRIMARY KEY (id),
        CONSTRAINT fk_applicant_achievements_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_certifications (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        certificate_name character varying(200) NOT NULL,
        issuing_body text NOT NULL,
        expiry_date timestamp with time zone,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_certifications PRIMARY KEY (id),
        CONSTRAINT fk_applicant_certifications_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_documents (
        id uuid NOT NULL,
        applicant_id uuid,
        document_type character varying(100) NOT NULL,
        file_url character varying(500) NOT NULL,
        uploaded_at timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_documents PRIMARY KEY (id),
        CONSTRAINT fk_applicant_documents_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_experiences (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        organization_name character varying(250) NOT NULL,
        designation character varying(150) NOT NULL,
        key_responsibilities character varying(2000),
        from_date timestamp with time zone NOT NULL,
        to_date timestamp with time zone,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_experiences PRIMARY KEY (id),
        CONSTRAINT fk_applicant_experiences_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_family_summaries (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        brothers_total integer NOT NULL,
        brothers_married integer NOT NULL,
        brothers_unmarried integer NOT NULL,
        sisters_total integer NOT NULL,
        sisters_married integer NOT NULL,
        sisters_unmarried integer NOT NULL,
        children_total integer NOT NULL,
        children_married integer NOT NULL,
        children_unmarried integer NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_family_summaries PRIMARY KEY (id),
        CONSTRAINT fk_applicant_family_summaries_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_financial_details (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        current_salary numeric(18,2),
        expected_salary numeric(18,2),
        other_benefits text,
        other_facilities text,
        family_income_detail character varying(100),
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_financial_details PRIMARY KEY (id),
        CONSTRAINT fk_applicant_financial_details_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_internal_relatives (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        relative_name character varying(200) NOT NULL,
        designation text NOT NULL,
        pay_scale text NOT NULL,
        department text NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_internal_relatives PRIMARY KEY (id),
        CONSTRAINT fk_applicant_internal_relatives_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_military_details (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        army_number character varying(50),
        army_unit character varying(100),
        army_character text,
        army_pay_scale text,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_military_details PRIMARY KEY (id),
        CONSTRAINT fk_applicant_military_details_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_personal_infos (
        id uuid NOT NULL,
        applicant_id uuid,
        candidate_type text NOT NULL,
        father_name character varying(200) NOT NULL,
        father_cnic text,
        date_of_birth timestamp with time zone NOT NULL,
        gender text NOT NULL,
        marital_status text NOT NULL,
        religion text NOT NULL,
        caste text,
        sect text,
        contact_no character varying(20) NOT NULL,
        email text,
        pec_number text,
        accommodation text,
        present_address text NOT NULL,
        permanent_address text NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_personal_infos PRIMARY KEY (id),
        CONSTRAINT fk_applicant_personal_infos_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_siblings (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        name character varying(200) NOT NULL,
        cnic character varying(15),
        date_of_birth timestamp with time zone NOT NULL,
        occupation text,
        organization text,
        gender text,
        marital_status text,
        designation text,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_siblings PRIMARY KEY (id),
        CONSTRAINT fk_applicant_siblings_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_skills (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        skill_name character varying(100) NOT NULL,
        proficiency text NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_skills PRIMARY KEY (id),
        CONSTRAINT fk_applicant_skills_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "AspNetRoleClaims" (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        role_id uuid NOT NULL,
        claim_type text,
        claim_value text,
        CONSTRAINT pk_asp_net_role_claims PRIMARY KEY (id),
        CONSTRAINT fk_asp_net_role_claims_asp_net_roles_role_id FOREIGN KEY (role_id) REFERENCES "AspNetRoles" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "Provinces" (
        id uuid NOT NULL,
        country_id uuid,
        name character varying(100) NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_provinces PRIMARY KEY (id),
        CONSTRAINT fk_provinces_countries_country_id FOREIGN KEY (country_id) REFERENCES countries (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE applicant_educations (
        id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        degree_level_id uuid,
        qualification character varying(100) NOT NULL,
        major_field text NOT NULL,
        board_university character varying(200) NOT NULL,
        cgpa_percentage text NOT NULL,
        from_date timestamp with time zone NOT NULL,
        to_date timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_applicant_educations PRIMARY KEY (id),
        CONSTRAINT fk_applicant_educations_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id),
        CONSTRAINT fk_applicant_educations_degree_levels_degree_level_id FOREIGN KEY (degree_level_id) REFERENCES degree_levels (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "AspNetUsers" (
        id uuid NOT NULL,
        full_name character varying(200) NOT NULL,
        designation_id uuid,
        department_id uuid,
        pay_scale_id uuid,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        user_name character varying(256),
        normalized_user_name character varying(256),
        email character varying(256),
        normalized_email character varying(256),
        email_confirmed boolean NOT NULL,
        password_hash text,
        security_stamp text,
        concurrency_stamp text,
        phone_number text,
        phone_number_confirmed boolean NOT NULL,
        two_factor_enabled boolean NOT NULL,
        lockout_end timestamp with time zone,
        lockout_enabled boolean NOT NULL,
        access_failed_count integer NOT NULL,
        CONSTRAINT pk_asp_net_users PRIMARY KEY (id),
        CONSTRAINT fk_asp_net_users_departments_department_id FOREIGN KEY (department_id) REFERENCES departments (id) ON DELETE RESTRICT,
        CONSTRAINT fk_asp_net_users_designations_designation_id FOREIGN KEY (designation_id) REFERENCES designations (id) ON DELETE RESTRICT,
        CONSTRAINT fk_asp_net_users_pay_scales_pay_scale_id FOREIGN KEY (pay_scale_id) REFERENCES "PayScales" (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE role_permissions (
        role_id uuid NOT NULL,
        permission_id uuid NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_role_permissions PRIMARY KEY (role_id, permission_id),
        CONSTRAINT fk_role_permissions_permissions_permission_id FOREIGN KEY (permission_id) REFERENCES permissions (id) ON DELETE CASCADE,
        CONSTRAINT fk_role_permissions_roles_role_id FOREIGN KEY (role_id) REFERENCES "AspNetRoles" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE campaign_export_tasks (
        id uuid NOT NULL,
        campaign_id uuid NOT NULL,
        requested_by_user_id uuid NOT NULL,
        status text NOT NULL,
        download_url text,
        error_message text,
        processed_at timestamp with time zone,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_campaign_export_tasks PRIMARY KEY (id),
        CONSTRAINT fk_campaign_export_tasks_recruitment_campaigns_campaign_id FOREIGN KEY (campaign_id) REFERENCES recruitment_campaigns (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE job_openings (
        id uuid NOT NULL,
        campaign_id uuid,
        title character varying(256) NOT NULL,
        job_category text,
        employment_type text,
        total_positions integer,
        description text NOT NULL,
        requirements text NOT NULL,
        benefits text,
        location_type character varying(50),
        work_location character varying(150),
        min_age integer,
        max_age integer,
        salary_grade text,
        required_experience_years numeric(18,2) NOT NULL,
        min_education_level text NOT NULL,
        required_major_field text,
        is_pec_required boolean NOT NULL,
        status character varying(50) NOT NULL,
        is_featured boolean NOT NULL,
        job_slug text,
        posted_at timestamp with time zone NOT NULL,
        expires_at timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_job_openings PRIMARY KEY (id),
        CONSTRAINT fk_job_openings_recruitment_campaigns_campaign_id FOREIGN KEY (campaign_id) REFERENCES recruitment_campaigns (id) ON DELETE SET NULL
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "Districts" (
        id uuid NOT NULL,
        province_id uuid,
        name character varying(100) NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_districts PRIMARY KEY (id),
        CONSTRAINT fk_districts_provinces_province_id FOREIGN KEY (province_id) REFERENCES "Provinces" (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "AspNetUserClaims" (
        id integer GENERATED BY DEFAULT AS IDENTITY,
        user_id uuid NOT NULL,
        claim_type text,
        claim_value text,
        CONSTRAINT pk_asp_net_user_claims PRIMARY KEY (id),
        CONSTRAINT fk_asp_net_user_claims_asp_net_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "AspNetUserLogins" (
        login_provider text NOT NULL,
        provider_key text NOT NULL,
        provider_display_name text,
        user_id uuid NOT NULL,
        CONSTRAINT pk_asp_net_user_logins PRIMARY KEY (login_provider, provider_key),
        CONSTRAINT fk_asp_net_user_logins_asp_net_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "AspNetUserRoles" (
        user_id uuid NOT NULL,
        role_id uuid NOT NULL,
        CONSTRAINT pk_asp_net_user_roles PRIMARY KEY (user_id, role_id),
        CONSTRAINT fk_asp_net_user_roles_asp_net_roles_role_id FOREIGN KEY (role_id) REFERENCES "AspNetRoles" (id) ON DELETE CASCADE,
        CONSTRAINT fk_asp_net_user_roles_asp_net_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "AspNetUserTokens" (
        user_id uuid NOT NULL,
        login_provider text NOT NULL,
        name text NOT NULL,
        value text,
        CONSTRAINT pk_asp_net_user_tokens PRIMARY KEY (user_id, login_provider, name),
        CONSTRAINT fk_asp_net_user_tokens_asp_net_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE user_notifications (
        id uuid NOT NULL,
        user_id uuid NOT NULL,
        title text NOT NULL,
        message text NOT NULL,
        action_url text,
        type text NOT NULL,
        is_read boolean NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_user_notifications PRIMARY KEY (id),
        CONSTRAINT fk_user_notifications_users_user_id FOREIGN KEY (user_id) REFERENCES "AspNetUsers" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE job_applications (
        id uuid NOT NULL,
        job_opening_id uuid NOT NULL,
        applicant_id uuid NOT NULL,
        status text NOT NULL,
        match_score numeric(5,2) NOT NULL,
        recruiter_remarks text,
        applied_at timestamp with time zone NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_job_applications PRIMARY KEY (id),
        CONSTRAINT fk_job_applications_applicants_applicant_id FOREIGN KEY (applicant_id) REFERENCES applicants (id),
        CONSTRAINT fk_job_applications_job_openings_job_opening_id FOREIGN KEY (job_opening_id) REFERENCES job_openings (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE job_skill_requirements (
        id uuid NOT NULL,
        job_opening_id uuid NOT NULL,
        skill_name character varying(100) NOT NULL,
        is_mandatory boolean NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_job_skill_requirements PRIMARY KEY (id),
        CONSTRAINT fk_job_skill_requirements_job_openings_job_opening_id FOREIGN KEY (job_opening_id) REFERENCES job_openings (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE "Tehsils" (
        id uuid NOT NULL,
        district_id uuid,
        name character varying(100) NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_tehsils PRIMARY KEY (id),
        CONSTRAINT fk_tehsils_districts_district_id FOREIGN KEY (district_id) REFERENCES "Districts" (id)
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE application_status_histories (
        id uuid NOT NULL,
        job_application_id uuid NOT NULL,
        status text NOT NULL,
        remarks text,
        changed_by_user_id uuid NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_application_status_histories PRIMARY KEY (id),
        CONSTRAINT fk_application_status_histories_job_applications_job_applicati FOREIGN KEY (job_application_id) REFERENCES job_applications (id) ON DELETE CASCADE,
        CONSTRAINT fk_application_status_histories_users_changed_by_user_id FOREIGN KEY (changed_by_user_id) REFERENCES "AspNetUsers" (id) ON DELETE RESTRICT
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE TABLE addresses (
        id uuid NOT NULL,
        country_id uuid NOT NULL,
        province_id uuid NOT NULL,
        district_id uuid NOT NULL,
        tehsil_id uuid NOT NULL,
        street_address text NOT NULL,
        city_id uuid NOT NULL,
        created_at timestamp with time zone NOT NULL,
        is_deleted boolean NOT NULL,
        deleted_at timestamp with time zone,
        CONSTRAINT pk_addresses PRIMARY KEY (id),
        CONSTRAINT fk_addresses_countries_country_id FOREIGN KEY (country_id) REFERENCES countries (id) ON DELETE CASCADE,
        CONSTRAINT fk_addresses_districts_district_id FOREIGN KEY (district_id) REFERENCES "Districts" (id) ON DELETE CASCADE,
        CONSTRAINT fk_addresses_provinces_province_id FOREIGN KEY (province_id) REFERENCES "Provinces" (id) ON DELETE CASCADE,
        CONSTRAINT fk_addresses_tehsils_city_id FOREIGN KEY (city_id) REFERENCES "Tehsils" (id) ON DELETE CASCADE
    );
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_addresses_city_id ON addresses (city_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_addresses_country_id ON addresses (country_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_addresses_district_id ON addresses (district_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_addresses_province_id ON addresses (province_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_achievements_applicant_id ON applicant_achievements (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_certifications_applicant_id ON applicant_certifications (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_documents_applicant_id ON applicant_documents (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_educations_applicant_id ON applicant_educations (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_educations_degree_level_id ON applicant_educations (degree_level_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_experiences_applicant_id ON applicant_experiences (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX ix_applicant_family_summaries_applicant_id ON applicant_family_summaries (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX ix_applicant_financial_details_applicant_id ON applicant_financial_details (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_internal_relatives_applicant_id ON applicant_internal_relatives (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX ix_applicant_military_details_applicant_id ON applicant_military_details (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX ix_applicant_personal_infos_applicant_id ON applicant_personal_infos (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_siblings_applicant_id ON applicant_siblings (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicant_skills_applicant_id ON applicant_skills (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_applicants_cnic_number ON applicants (cnic_number);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX ix_applicants_tracking_code ON applicants (tracking_code);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_application_status_histories_changed_by_user_id ON application_status_histories (changed_by_user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_application_status_histories_job_application_id ON application_status_histories (job_application_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_asp_net_role_claims_role_id ON "AspNetRoleClaims" (role_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX "RoleNameIndex" ON "AspNetRoles" (normalized_name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_asp_net_user_claims_user_id ON "AspNetUserClaims" (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_asp_net_user_logins_user_id ON "AspNetUserLogins" (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_asp_net_user_roles_role_id ON "AspNetUserRoles" (role_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX "EmailIndex" ON "AspNetUsers" (normalized_email);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_asp_net_users_department_id ON "AspNetUsers" (department_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_asp_net_users_designation_id ON "AspNetUsers" (designation_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_asp_net_users_pay_scale_id ON "AspNetUsers" (pay_scale_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX "UserNameIndex" ON "AspNetUsers" (normalized_user_name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_campaign_export_tasks_campaign_id ON campaign_export_tasks (campaign_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_districts_province_id ON "Districts" (province_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_job_applications_applicant_id ON job_applications (applicant_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_job_applications_job_opening_id ON job_applications (job_opening_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_job_openings_campaign_id ON job_openings (campaign_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_job_openings_expires_at ON job_openings (expires_at);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_job_openings_posted_at ON job_openings (posted_at);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_job_openings_status ON job_openings (status);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_job_skill_requirements_job_opening_id ON job_skill_requirements (job_opening_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX ix_permissions_name ON permissions (name);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_provinces_country_id ON "Provinces" (country_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE UNIQUE INDEX ix_recruitment_campaigns_campaign_code ON recruitment_campaigns (campaign_code);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_role_permissions_permission_id ON role_permissions (permission_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_tehsils_district_id ON "Tehsils" (district_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    CREATE INDEX ix_user_notifications_user_id ON user_notifications (user_id);
    END IF;
END $EF$;

DO $EF$
BEGIN
    IF NOT EXISTS(SELECT 1 FROM "__EFMigrationsHistory" WHERE "migration_id" = '20260425061725_InitialPostgresNaming') THEN
    INSERT INTO "__EFMigrationsHistory" (migration_id, product_version)
    VALUES ('20260425061725_InitialPostgresNaming', '10.0.7');
    END IF;
END $EF$;
COMMIT;

