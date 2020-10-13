using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class AddSpcAgentsAssignmentEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SpcAgentAvailabilityAndSkillsId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SpcLastTicketAssignedAt",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SpcAgentsAvailabilityAndSkills",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsAvailable = table.Column<bool>(nullable: false),
                    Urgent = table.Column<bool>(nullable: false),
                    Immediate = table.Column<bool>(nullable: false),
                    Normal = table.Column<bool>(nullable: false),
                    Low = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpcAgentsAvailabilityAndSkills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SpcAgentsAvailabilityAndSkillsChangeLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ChangedByUserId = table.Column<string>(nullable: false),
                    ChangedSkill = table.Column<string>(nullable: false),
                    OldValue = table.Column<bool>(nullable: false),
                    NewValue = table.Column<bool>(nullable: false),
                    ChangedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpcAgentsAvailabilityAndSkillsChangeLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpcAgentsAvailabilityAndSkillsChangeLogs_AspNetUsers_ChangedByUserId",
                        column: x => x.ChangedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SpcAgentsAvailabilityAndSkillsChangeLogs_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_SpcAgentAvailabilityAndSkillsId",
                table: "AspNetUsers",
                column: "SpcAgentAvailabilityAndSkillsId");

            migrationBuilder.CreateIndex(
                name: "IX_SpcAgentsAvailabilityAndSkillsChangeLogs_ChangedByUserId",
                table: "SpcAgentsAvailabilityAndSkillsChangeLogs",
                column: "ChangedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SpcAgentsAvailabilityAndSkillsChangeLogs_UserId",
                table: "SpcAgentsAvailabilityAndSkillsChangeLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_SpcAgentsAvailabilityAndSkills_SpcAgentAvailabilityAndSkillsId",
                table: "AspNetUsers",
                column: "SpcAgentAvailabilityAndSkillsId",
                principalTable: "SpcAgentsAvailabilityAndSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_SpcAgentsAvailabilityAndSkills_SpcAgentAvailabilityAndSkillsId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "SpcAgentsAvailabilityAndSkills");

            migrationBuilder.DropTable(
                name: "SpcAgentsAvailabilityAndSkillsChangeLogs");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_SpcAgentAvailabilityAndSkillsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpcAgentAvailabilityAndSkillsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "SpcLastTicketAssignedAt",
                table: "AspNetUsers");
        }
    }
}
