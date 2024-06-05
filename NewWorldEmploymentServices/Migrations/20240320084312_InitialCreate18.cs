using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewWorldEmploymentServices.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate18 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkExperience",
                table: "PostJobs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "PostJobs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Skills",
                table: "PostJobs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Education",
                table: "PostJobs");

            migrationBuilder.DropColumn(
                name: "Skills",
                table: "PostJobs");

            migrationBuilder.AlterColumn<int>(
                name: "WorkExperience",
                table: "PostJobs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
