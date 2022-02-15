using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace comp2001hk_assessment2_part1.Migrations
{
    public partial class ChangeDbSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "cw2");

            migrationBuilder.RenameTable(
                name: "Students",
                newName: "Students",
                newSchema: "cw2");

            migrationBuilder.RenameTable(
                name: "ProgrammeStudentAudit",
                newName: "ProgrammeStudentAudit",
                newSchema: "cw2");

            migrationBuilder.RenameTable(
                name: "ProgrammeStudent",
                newName: "ProgrammeStudent",
                newSchema: "cw2");

            migrationBuilder.RenameTable(
                name: "Programmes",
                newName: "Programmes",
                newSchema: "cw2");

            migrationBuilder.RenameTable(
                name: "ProgrammeAudit",
                newName: "ProgrammeAudit",
                newSchema: "cw2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Students",
                schema: "cw2",
                newName: "Students");

            migrationBuilder.RenameTable(
                name: "ProgrammeStudentAudit",
                schema: "cw2",
                newName: "ProgrammeStudentAudit");

            migrationBuilder.RenameTable(
                name: "ProgrammeStudent",
                schema: "cw2",
                newName: "ProgrammeStudent");

            migrationBuilder.RenameTable(
                name: "Programmes",
                schema: "cw2",
                newName: "Programmes");

            migrationBuilder.RenameTable(
                name: "ProgrammeAudit",
                schema: "cw2",
                newName: "ProgrammeAudit");
        }
    }
}
