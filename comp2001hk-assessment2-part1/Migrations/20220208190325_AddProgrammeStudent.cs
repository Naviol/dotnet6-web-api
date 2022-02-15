using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace comp2001hk_assessment2_part1.Migrations
{
    public partial class AddProgrammeStudent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Students_Programmes_ProgrammeId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Students_ProgrammeId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ProgrammeId",
                table: "Students");

            migrationBuilder.CreateTable(
                name: "ProgrammeStudent",
                columns: table => new
                {
                    ProgrammesId = table.Column<int>(type: "int", nullable: false),
                    StudentsStudent_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgrammeStudent", x => new { x.ProgrammesId, x.StudentsStudent_id });
                    table.ForeignKey(
                        name: "FK_ProgrammeStudent_Programmes_ProgrammesId",
                        column: x => x.ProgrammesId,
                        principalTable: "Programmes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgrammeStudent_Students_StudentsStudent_id",
                        column: x => x.StudentsStudent_id,
                        principalTable: "Students",
                        principalColumn: "Student_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgrammeStudent_StudentsStudent_id",
                table: "ProgrammeStudent",
                column: "StudentsStudent_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgrammeStudent");

            migrationBuilder.AddColumn<int>(
                name: "ProgrammeId",
                table: "Students",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_ProgrammeId",
                table: "Students",
                column: "ProgrammeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_Programmes_ProgrammeId",
                table: "Students",
                column: "ProgrammeId",
                principalTable: "Programmes",
                principalColumn: "Id");

        }
    }
}
