using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorldEmploymentServices.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate16 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContactNo",
                table: "PostJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "PostJobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContactNo",
                table: "PostJobs");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "PostJobs");
        }
    }
}
