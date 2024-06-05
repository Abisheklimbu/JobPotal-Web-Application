using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorldEmploymentServices.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostJobs_Organizations_PermanentOrganizationId",
                table: "PostJobs");

            migrationBuilder.DropColumn(
                name: "PermanentOrganizationId",
                table: "Organizations");

            migrationBuilder.RenameColumn(
                name: "PermanentOrganizationId",
                table: "PostJobs",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_PostJobs_PermanentOrganizationId",
                table: "PostJobs",
                newName: "IX_PostJobs_OrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PostJobs_Organizations_OrganizationId",
                table: "PostJobs",
                column: "OrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostJobs_Organizations_OrganizationId",
                table: "PostJobs");

            migrationBuilder.RenameColumn(
                name: "OrganizationId",
                table: "PostJobs",
                newName: "PermanentOrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_PostJobs_OrganizationId",
                table: "PostJobs",
                newName: "IX_PostJobs_PermanentOrganizationId");

            migrationBuilder.AddColumn<int>(
                name: "PermanentOrganizationId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PostJobs_Organizations_PermanentOrganizationId",
                table: "PostJobs",
                column: "PermanentOrganizationId",
                principalTable: "Organizations",
                principalColumn: "OrganizationId");
        }
    }
}
