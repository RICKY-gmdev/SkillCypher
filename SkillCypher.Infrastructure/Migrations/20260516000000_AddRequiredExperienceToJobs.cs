using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCypher.Infrastructure.Migrations
{
    public partial class AddRequiredExperienceToJobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RequiredExperienceYears",
                table: "Jobs",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RequiredExperienceYears",
                table: "Jobs");
        }
    }
}