-- 1. Create a dedicated user for the application
CREATE USER career_admin WITH PASSWORD 'CareerSecure@635!';

-- 2. Create the database
CREATE DATABASE "Career635DB" OWNER career_admin;

-- 3. Grant permissions
GRANT ALL PRIVILEGES ON DATABASE "Career635DB" TO career_admin;

-- Connect to the database to grant schema-level permissions
\c "Career635DB"
GRANT ALL ON SCHEMA public TO career_admin;



-- IIS App Pool:
-- Right-click "Application Pools" -> "Add Application Pool".
-- Name: Career635Pool.
-- .NET CLR Version: No Managed Code (Crucial for .NET Core+).
-- Permissions:
-- Ensure the folder where your project lives (e.g., C:\inetpub\wwwroot\Career635) has Read & Execute permissions for the user IIS AppPool\Career635Pool.
--  "DatabaseProvider": "Postgres",
--   "ConnectionStrings": {
--     "DefaultConnection": "Host=localhost;Port=5432;Database=Career635DB;Username=career_admin;Password=CareerSecure@635!;Include Error Detail=true"
--   },

-- # 1. Clean previous migrations if they were for SQL Server
-- # 2. Add fresh Postgres migration
-- dotnet ef migrations add InitialPostgres -o Infrastructure/Persistence/Migrations/Postgres --context AppDbContext

-- # 3. Apply the schema to your fresh DB
-- dotnet ef database update --context AppDbContext