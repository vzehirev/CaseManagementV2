using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class RemoveChangedByUserFromCaseAndTaskEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Tasks");

            migrationBuilder.DropColumn(
                name: "ChangedByUser",
                table: "Cases");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChangedByUser",
                table: "Cases",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
