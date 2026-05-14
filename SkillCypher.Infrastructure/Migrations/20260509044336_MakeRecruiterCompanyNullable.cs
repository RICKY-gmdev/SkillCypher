using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkillCypher.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeRecruiterCompanyNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recruiters_Companies_CompanyID",
                table: "Recruiters");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "Recruiters",
                newName: "CompanyId");

            migrationBuilder.RenameIndex(
                name: "IX_Recruiters_CompanyID",
                table: "Recruiters",
                newName: "IX_Recruiters_CompanyId");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Recruiters",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Recruiters_Companies_CompanyId",
                table: "Recruiters",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recruiters_Companies_CompanyId",
                table: "Recruiters");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Recruiters",
                newName: "CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_Recruiters_CompanyId",
                table: "Recruiters",
                newName: "IX_Recruiters_CompanyID");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyID",
                table: "Recruiters",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Recruiters_Companies_CompanyID",
                table: "Recruiters",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
