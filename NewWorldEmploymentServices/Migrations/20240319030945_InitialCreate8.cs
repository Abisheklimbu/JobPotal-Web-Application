using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorldEmploymentServices.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PostJobs_Organizations_OrganizationIds",
                table: "PostJobs");

            migrationBuilder.RenameColumn(
                name: "OrganizationIds",
                table: "PostJobs",
                newName: "OrganizationId");

            migrationBuilder.RenameIndex(
                name: "IX_PostJobs_OrganizationIds",
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
                newName: "OrganizationIds");

            migrationBuilder.RenameIndex(
                name: "IX_PostJobs_OrganizationId",
                table: "PostJobs",
                newName: "IX_PostJobs_OrganizationIds");

            migrationBuilder.AddForeignKey(
                name: "FK_PostJobs_Organizations_OrganizationIds",
                table: "PostJobs",
                column: "OrganizationIds",
                principalTable: "Organizations",
                principalColumn: "OrganizationId");
        }
    }
}
