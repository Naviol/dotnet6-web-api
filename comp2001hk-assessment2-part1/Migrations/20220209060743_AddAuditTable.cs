using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace comp2001hk_assessment2_part1.Migrations
{
    public partial class AddAuditTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgrammeAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Programme_id = table.Column<int>(type: "int", nullable: false),
                    Programme_code_before = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Programme_code_after = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Programme_title_before = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Programmeme_title_after = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammeAudit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProgrammeStudentAudit",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Operation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProgrammeId_before = table.Column<int>(type: "int", nullable: false),
                    ProgrammeId_after = table.Column<int>(type: "int", nullable: false),
                    StudentId_before = table.Column<int>(type: "int", nullable: false),
                    StudentId_after = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammeStudentAudit", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgrammeAudit");

            migrationBuilder.DropTable(
                name: "ProgrammeStudentAudit");
        }
    }
}
