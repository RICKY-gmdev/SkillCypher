using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCypher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixMissingJobDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
                migrationBuilder.AddColumn<string>(
                    name: "Description",
                    table: "Jobs",
                    type: "text",
                    nullable: false,
                    defaultValue: "");
                
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Jobs"
            );
        }
    }
}
