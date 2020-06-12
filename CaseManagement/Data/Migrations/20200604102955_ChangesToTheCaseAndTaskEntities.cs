using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class ChangesToTheCaseAndTaskEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AspNetUsers_UserId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseModificationLogRecords_Cases_CaseId",
                table: "CaseModificationLogRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseModificationLogRecords_AspNetUsers_UserId",
                table: "CaseModificationLogRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CasePriorities_PriorityId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CaseStatuses_StatusId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Datacenters_TimeZoneRegions_TimeZoneRegionId",
                table: "Datacenters");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Vendors_VendorId",
                table: "Regions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskModificationLogRecords_Tasks_TaskId",
                table: "TaskModificationLogRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskModificationLogRecords_AspNetUsers_UserId",
                table: "TaskModificationLogRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Cases_CaseId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_CasePriorities_PriorityId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "QueueStatus",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "WaitingReason",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "CaseTypes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CaseStatuses");

            migrationBuilder.DropColumn(
                name: "QueueStatus",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "WaitingReason",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Priority",
                table: "CasePriorities");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Tasks",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QueueStatusId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaitingReasonId",
                table: "Tasks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaseTypeName",
                table: "CaseTypes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CaseStatusName",
                table: "CaseStatuses",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Cases",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "QueueStatusId",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WaitingReasonId",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CasePriorityName",
                table: "CasePriorities",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "QueueStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QueueStatusName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueueStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WaitingReasons",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WaitingReasonName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WaitingReasons", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_QueueStatusId",
                table: "Tasks",
                column: "QueueStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_WaitingReasonId",
                table: "Tasks",
                column: "WaitingReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_QueueStatusId",
                table: "Cases",
                column: "QueueStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_WaitingReasonId",
                table: "Cases",
                column: "WaitingReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AspNetUsers_UserId",
                table: "Announcements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseModificationLogRecords_Cases_CaseId",
                table: "CaseModificationLogRecords",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseModificationLogRecords_AspNetUsers_UserId",
                table: "CaseModificationLogRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CasePriorities_PriorityId",
                table: "Cases",
                column: "PriorityId",
                principalTable: "CasePriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_QueueStatuses_QueueStatusId",
                table: "Cases",
                column: "QueueStatusId",
                principalTable: "QueueStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CaseStatuses_StatusId",
                table: "Cases",
                column: "StatusId",
                principalTable: "CaseStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_WaitingReasons_WaitingReasonId",
                table: "Cases",
                column: "WaitingReasonId",
                principalTable: "WaitingReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Datacenters_TimeZoneRegions_TimeZoneRegionId",
                table: "Datacenters",
                column: "TimeZoneRegionId",
                principalTable: "TimeZoneRegions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Vendors_VendorId",
                table: "Regions",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModificationLogRecords_Tasks_TaskId",
                table: "TaskModificationLogRecords",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModificationLogRecords_AspNetUsers_UserId",
                table: "TaskModificationLogRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Cases_CaseId",
                table: "Tasks",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_CasePriorities_PriorityId",
                table: "Tasks",
                column: "PriorityId",
                principalTable: "CasePriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_QueueStatuses_QueueStatusId",
                table: "Tasks",
                column: "QueueStatusId",
                principalTable: "QueueStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_WaitingReasons_WaitingReasonId",
                table: "Tasks",
                column: "WaitingReasonId",
                principalTable: "WaitingReasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_AspNetUsers_UserId",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseModificationLogRecords_Cases_CaseId",
                table: "CaseModificationLogRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_CaseModificationLogRecords_AspNetUsers_UserId",
                table: "CaseModificationLogRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CasePriorities_PriorityId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_QueueStatuses_QueueStatusId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CaseStatuses_StatusId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_WaitingReasons_WaitingReasonId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Datacenters_TimeZoneRegions_TimeZoneRegionId",
                table: "Datacenters");

            migrationBuilder.DropForeignKey(
                name: "FK_Regions_Vendors_VendorId",
                table: "Regions");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskModificationLogRecords_Tasks_TaskId",
                table: "TaskModificationLogRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskModificationLogRecords_AspNetUsers_UserId",
                table: "TaskModificationLogRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Cases_CaseId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_CasePriorities_PriorityId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_QueueStatuses_QueueStatusId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_WaitingReasons_WaitingReasonId",
                table: "Tasks");

            migrationBuilder.DropTable(
                name: "QueueStatuses");

            migrationBuilder.DropTable(
                name: "WaitingReasons");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_QueueStatusId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Tasks_WaitingReasonId",
                table: "Tasks");

            migrationBuilder.DropIndex(
                name: "IX_Cases_QueueStatusId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_WaitingReasonId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "QueueStatusId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "WaitingReasonId",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "CaseTypeName",
                table: "CaseTypes");

            migrationBuilder.DropColumn(
                name: "CaseStatusName",
                table: "CaseStatuses");

            migrationBuilder.DropColumn(
                name: "QueueStatusId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "WaitingReasonId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CasePriorityName",
                table: "CasePriorities");

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Tasks",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Tasks",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddColumn<string>(
                name: "QueueStatus",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaitingReason",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "CaseTypes",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CaseStatuses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "TypeId",
                table: "Cases",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "QueueStatus",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaitingReason",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Priority",
                table: "CasePriorities",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_AspNetUsers_UserId",
                table: "Announcements",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseModificationLogRecords_Cases_CaseId",
                table: "CaseModificationLogRecords",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CaseModificationLogRecords_AspNetUsers_UserId",
                table: "CaseModificationLogRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CasePriorities_PriorityId",
                table: "Cases",
                column: "PriorityId",
                principalTable: "CasePriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CaseStatuses_StatusId",
                table: "Cases",
                column: "StatusId",
                principalTable: "CaseStatuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Datacenters_TimeZoneRegions_TimeZoneRegionId",
                table: "Datacenters",
                column: "TimeZoneRegionId",
                principalTable: "TimeZoneRegions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Regions_Vendors_VendorId",
                table: "Regions",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModificationLogRecords_Tasks_TaskId",
                table: "TaskModificationLogRecords",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskModificationLogRecords_AspNetUsers_UserId",
                table: "TaskModificationLogRecords",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Cases_CaseId",
                table: "Tasks",
                column: "CaseId",
                principalTable: "Cases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_CasePriorities_PriorityId",
                table: "Tasks",
                column: "PriorityId",
                principalTable: "CasePriorities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
