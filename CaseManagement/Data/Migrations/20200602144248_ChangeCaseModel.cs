using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class ChangeCaseModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cases_CasePhases_PhaseId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_ServiceAreas_ServiceAreaId",
                table: "Cases");

            migrationBuilder.DropForeignKey(
                name: "FK_Cases_Services_ServiceId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_PhaseId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_ServiceAreaId",
                table: "Cases");

            migrationBuilder.DropIndex(
                name: "IX_Cases_ServiceId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "EndTime",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "PhaseId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "ServiceAreaId",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "Cases");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Cases",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "AssignedProcessor",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangedByUser",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastUpdatedUtc",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Queue",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QueueStatus",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReportedAt",
                table: "Cases",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "ResumeAt",
                table: "Cases",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WaitingReason",
                table: "Cases",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedProcessor",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "ChangedByUser",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "LastUpdatedUtc",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "Queue",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "QueueStatus",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "ReportedAt",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "ResumeAt",
                table: "Cases");

            migrationBuilder.DropColumn(
                name: "WaitingReason",
                table: "Cases");

            migrationBuilder.AlterColumn<string>(
                name: "Subject",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Cases",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndTime",
                table: "Cases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Cases",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PhaseId",
                table: "Cases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceAreaId",
                table: "Cases",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "Cases",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cases_PhaseId",
                table: "Cases",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ServiceAreaId",
                table: "Cases",
                column: "ServiceAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Cases_ServiceId",
                table: "Cases",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_CasePhases_PhaseId",
                table: "Cases",
                column: "PhaseId",
                principalTable: "CasePhases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_ServiceAreas_ServiceAreaId",
                table: "Cases",
                column: "ServiceAreaId",
                principalTable: "ServiceAreas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Cases_Services_ServiceId",
                table: "Cases",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
