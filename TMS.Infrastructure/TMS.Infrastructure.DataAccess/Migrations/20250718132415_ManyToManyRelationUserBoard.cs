using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TMS.Infrastructure.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyRelationUserBoard : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Companies_CompanyId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_HeadId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Boards_BoardId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BoardId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BoardId",
                table: "Users");

            migrationBuilder.CreateTable(
                name: "BoardUser",
                columns: table => new
                {
                    BoardsId = table.Column<Guid>(type: "uuid", nullable: false),
                    UsersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoardUser", x => new { x.BoardsId, x.UsersId });
                    table.ForeignKey(
                        name: "FK_BoardUser_Boards_BoardsId",
                        column: x => x.BoardsId,
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoardUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoardUser_UsersId",
                table: "BoardUser",
                column: "UsersId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Companies_CompanyId",
                table: "Boards",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_HeadId",
                table: "Boards",
                column: "HeadId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Companies_CompanyId",
                table: "Boards");

            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Users_HeadId",
                table: "Boards");

            migrationBuilder.DropTable(
                name: "BoardUser");

            migrationBuilder.AddColumn<Guid>(
                name: "BoardId",
                table: "Users",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BoardId",
                table: "Users",
                column: "BoardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Companies_CompanyId",
                table: "Boards",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Users_HeadId",
                table: "Boards",
                column: "HeadId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Boards_BoardId",
                table: "Users",
                column: "BoardId",
                principalTable: "Boards",
                principalColumn: "Id");
        }
    }
}
