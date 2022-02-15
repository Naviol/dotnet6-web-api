using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace comp2001hk_assessment2_part1.Migrations
{
    public partial class RenameTableTypoColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Programmeme_title_after",
                table: "ProgrammeAudit",
                newName: "Programme_title_after");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Programme_title_after",
                table: "ProgrammeAudit",
                newName: "Programmeme_title_after");
        }
    }
}
