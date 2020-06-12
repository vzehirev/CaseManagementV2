using Microsoft.EntityFrameworkCore.Migrations;

namespace CaseManagement.Data.Migrations
{
    public partial class ChangeTaskChangedByFieldName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedByUser",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Tasks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Tasks");

            migrationBuilder.AddColumn<string>(
                name: "ChangedByUser",
                table: "Tasks",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
