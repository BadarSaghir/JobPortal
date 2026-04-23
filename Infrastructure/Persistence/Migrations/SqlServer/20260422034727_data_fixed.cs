using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Career635.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class data_fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "ApplicantSiblings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "ApplicantSiblings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MaritalStatus",
                table: "ApplicantSiblings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Accommodation",
                table: "ApplicantPersonalInfos",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherFacilities",
                table: "ApplicantFinancialDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "ApplicantSiblings");

            migrationBuilder.DropColumn(
                name: "Gender",
                table: "ApplicantSiblings");

            migrationBuilder.DropColumn(
                name: "MaritalStatus",
                table: "ApplicantSiblings");

            migrationBuilder.DropColumn(
                name: "Accommodation",
                table: "ApplicantPersonalInfos");

            migrationBuilder.DropColumn(
                name: "OtherFacilities",
                table: "ApplicantFinancialDetails");
        }
    }
}
