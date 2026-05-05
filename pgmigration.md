# Generate
dotnet ef migrations add InitialPostgres -o Infrastructure/Persistence/Migrations/Postgres --context PostgresDbContext

# Apply
dotnet ef database update --context PostgresDbContext