# Creation for MSSQL
dotnet ef migrations add <MigrationName> -o Infrastructure/Persistence/Migrations/SqlServer --context AppDbContext
# Migrations
dotnet ef database update --context AppDbContext