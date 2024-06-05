using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorldEmploymentServices.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JobListings");

            migrationBuilder.DropIndex(
                name: "IX_PostJobs_JobNatureId",
                table: "PostJobs");

            migrationBuilder.AddColumn<int>(
                name: "PermanentOrganizationId",
                table: "Organizations",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PostJobs_JobNatureId",
                table: "PostJobs",
                column: "JobNatureId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PostJobs_JobNatureId",
                table: "PostJobs");

            migrationBuilder.DropColumn(
                name: "PermanentOrganizationId",
                table: "Organizations");

            migrationBuilder.CreateTable(
                name: "JobListings",
                columns: table => new
                {
                    JobListId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JobId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobListings", x => x.JobListId);
                    table.ForeignKey(
                        name: "FK_JobListings_PostJobs_JobId",
                        column: x => x.JobId,
                        principalTable: "PostJobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostJobs_JobNatureId",
                table: "PostJobs",
                column: "JobNatureId",
                unique: true,
                filter: "[JobNatureId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobListings_JobId",
                table: "JobListings",
                column: "JobId");
        }
    }
}
