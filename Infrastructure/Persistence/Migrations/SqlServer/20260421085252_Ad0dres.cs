using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Career635.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class Ad0dres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantAchievements_Applicants_ApplicantId",
                table: "ApplicantAchievements");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantCertifications_Applicants_ApplicantId",
                table: "ApplicantCertifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantDocuments_Applicants_ApplicantId1",
                table: "ApplicantDocuments");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantEducations_Applicants_ApplicantId1",
                table: "ApplicantEducations");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantExperiences_Applicants_ApplicantId1",
                table: "ApplicantExperiences");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantInternalRelatives_Applicants_ApplicantId1",
                table: "ApplicantInternalRelatives");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSkills_Applicants_ApplicantId",
                table: "ApplicantSkills");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantPersonalInfos_ApplicantId",
                table: "ApplicantPersonalInfos");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantInternalRelatives_ApplicantId1",
                table: "ApplicantInternalRelatives");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantExperiences_ApplicantId1",
                table: "ApplicantExperiences");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantEducations_ApplicantId1",
                table: "ApplicantEducations");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantDocuments_ApplicantId1",
                table: "ApplicantDocuments");

            migrationBuilder.DropColumn(
                name: "Department",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "ApplicantId1",
                table: "ApplicantInternalRelatives");

            migrationBuilder.DropColumn(
                name: "ApplicantId1",
                table: "ApplicantExperiences");

            migrationBuilder.DropColumn(
                name: "ApplicantId1",
                table: "ApplicantEducations");

            migrationBuilder.DropColumn(
                name: "ApplicantId1",
                table: "ApplicantDocuments");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicantId",
                table: "ApplicantPersonalInfos",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicantId",
                table: "ApplicantInternalRelatives",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "DegreeLevelId",
                table: "ApplicantEducations",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicantId",
                table: "ApplicantDocuments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantPersonalInfos_ApplicantId",
                table: "ApplicantPersonalInfos",
                column: "ApplicantId",
                unique: true,
                filter: "[ApplicantId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantAchievements_Applicants_ApplicantId",
                table: "ApplicantAchievements",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantCertifications_Applicants_ApplicantId",
                table: "ApplicantCertifications",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSkills_Applicants_ApplicantId",
                table: "ApplicantSkills",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantAchievements_Applicants_ApplicantId",
                table: "ApplicantAchievements");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantCertifications_Applicants_ApplicantId",
                table: "ApplicantCertifications");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicantSkills_Applicants_ApplicantId",
                table: "ApplicantSkills");

            migrationBuilder.DropIndex(
                name: "IX_ApplicantPersonalInfos_ApplicantId",
                table: "ApplicantPersonalInfos");

            migrationBuilder.AddColumn<string>(
                name: "Department",
                table: "JobOpenings",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicantId",
                table: "ApplicantPersonalInfos",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicantId",
                table: "ApplicantInternalRelatives",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantId1",
                table: "ApplicantInternalRelatives",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantId1",
                table: "ApplicantExperiences",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "DegreeLevelId",
                table: "ApplicantEducations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantId1",
                table: "ApplicantEducations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<Guid>(
                name: "ApplicantId",
                table: "ApplicantDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantId1",
                table: "ApplicantDocuments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantPersonalInfos_ApplicantId",
                table: "ApplicantPersonalInfos",
                column: "ApplicantId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantInternalRelatives_ApplicantId1",
                table: "ApplicantInternalRelatives",
                column: "ApplicantId1");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantExperiences_ApplicantId1",
                table: "ApplicantExperiences",
                column: "ApplicantId1");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantEducations_ApplicantId1",
                table: "ApplicantEducations",
                column: "ApplicantId1");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicantDocuments_ApplicantId1",
                table: "ApplicantDocuments",
                column: "ApplicantId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantAchievements_Applicants_ApplicantId",
                table: "ApplicantAchievements",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantCertifications_Applicants_ApplicantId",
                table: "ApplicantCertifications",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantDocuments_Applicants_ApplicantId1",
                table: "ApplicantDocuments",
                column: "ApplicantId1",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantEducations_Applicants_ApplicantId1",
                table: "ApplicantEducations",
                column: "ApplicantId1",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantExperiences_Applicants_ApplicantId1",
                table: "ApplicantExperiences",
                column: "ApplicantId1",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantInternalRelatives_Applicants_ApplicantId1",
                table: "ApplicantInternalRelatives",
                column: "ApplicantId1",
                principalTable: "Applicants",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicantSkills_Applicants_ApplicantId",
                table: "ApplicantSkills",
                column: "ApplicantId",
                principalTable: "Applicants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
