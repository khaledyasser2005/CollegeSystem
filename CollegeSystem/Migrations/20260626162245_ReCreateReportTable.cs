using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeSystem.Migrations
{
    public partial class ReCreateReportTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseID",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CourseID",
                table: "Reports",
                column: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Courses_CourseID",
                table: "Reports",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Courses_CourseID",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CourseID",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CourseID",
                table: "Reports");
        }
    }
}