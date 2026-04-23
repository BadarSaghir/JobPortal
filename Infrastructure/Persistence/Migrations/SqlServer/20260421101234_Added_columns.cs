using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Career635.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class Added_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Benefits",
                table: "JobOpenings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmploymentType",
                table: "JobOpenings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFeatured",
                table: "JobOpenings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JobCategory",
                table: "JobOpenings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobSlug",
                table: "JobOpenings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SalaryGrade",
                table: "JobOpenings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalPositions",
                table: "JobOpenings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Benefits",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "EmploymentType",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "IsFeatured",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "JobCategory",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "JobSlug",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "SalaryGrade",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "TotalPositions",
                table: "JobOpenings");
        }
    }
}
