using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class ChangeTaskModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskStatuses_StatusId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_TaskTypes_TypeId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Action",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "NextAction",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "AssignedProcessor",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangedByUser",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriorityId",
                table: "Tasks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Queue",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueueStatus",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportedAt",
                table: "Tasks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ResumeAt",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subject",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaitingReason",
                table: "Tasks",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_PriorityId",
                table: "Tasks",
                column: "PriorityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_CasePriorities_PriorityId",
                table: "Tasks",
                column: "PriorityId",
                principalTable: "CasePriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_CaseStatuses_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "CaseStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_CaseTypes_TypeId",
                table: "Tasks",
                column: "TypeId",
                principalTable: "CaseTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_CasePriorities_PriorityId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_CaseStatuses_StatusId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_CaseTypes_TypeId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_PriorityId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "AssignedProcessor",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ChangedByUser",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "PriorityId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Queue",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "QueueStatus",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ReportedAt",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ResumeAt",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Subject",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "WaitingReason",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "Action",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Tasks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Tasks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NextAction",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskStatuses_StatusId",
                table: "Tasks",
                column: "StatusId",
                principalTable: "TaskStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_TaskTypes_TypeId",
                table: "Tasks",
                column: "TypeId",
                principalTable: "TaskTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
