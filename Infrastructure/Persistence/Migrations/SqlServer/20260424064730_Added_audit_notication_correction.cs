using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Career635.Infrastructure.Persistence.Migrations.SqlServer
{
    /// <inheritdoc />
    public partial class Added_audit_notication_correction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_userNotifications_Users_UserId",
                table: "userNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_userNotifications",
                table: "userNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_auditLogs",
                table: "auditLogs");

            migrationBuilder.RenameTable(
                name: "userNotifications",
                newName: "UserNotifications");

            migrationBuilder.RenameTable(
                name: "auditLogs",
                newName: "AuditLogs");

            migrationBuilder.RenameIndex(
                name: "IX_userNotifications_UserId",
                table: "UserNotifications",
                newName: "IX_UserNotifications_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserNotifications",
                table: "UserNotifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_UserNotifications_Users_UserId",
                table: "UserNotifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserNotifications_Users_UserId",
                table: "UserNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserNotifications",
                table: "UserNotifications");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AuditLogs",
                table: "AuditLogs");

            migrationBuilder.RenameTable(
                name: "UserNotifications",
                newName: "userNotifications");

            migrationBuilder.RenameTable(
                name: "AuditLogs",
                newName: "auditLogs");

            migrationBuilder.RenameIndex(
                name: "IX_UserNotifications_UserId",
                table: "userNotifications",
                newName: "IX_userNotifications_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_userNotifications",
                table: "userNotifications",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_auditLogs",
                table: "auditLogs",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_userNotifications_Users_UserId",
                table: "userNotifications",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
