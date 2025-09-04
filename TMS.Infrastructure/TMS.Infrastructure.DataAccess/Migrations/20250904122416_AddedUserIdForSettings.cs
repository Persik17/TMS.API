using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdForSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_NotificationSettings_NotificationSettingsId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_NotificationSettingsId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "NotificationSettings",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings",
                column: "UserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationSettings_Users_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropIndex(
                name: "IX_NotificationSettings_UserId",
                table: "NotificationSettings");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "NotificationSettings");

            migrationBuilder.CreateIndex(
                name: "IX_Users_NotificationSettingsId",
                table: "Users",
                column: "NotificationSettingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_NotificationSettings_NotificationSettingsId",
                table: "Users",
                column: "NotificationSettingsId",
                principalTable: "NotificationSettings",
                principalColumn: "Id");
        }
    }
}
