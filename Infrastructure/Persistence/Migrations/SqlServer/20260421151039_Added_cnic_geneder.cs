using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Career635.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class Added_cnic_geneder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gender",
                table: "ApplicantPersonalInfos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "ApplicantPersonalInfos");
        }
    }
}
