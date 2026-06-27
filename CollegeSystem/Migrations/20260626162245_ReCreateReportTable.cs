using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeSystem.Migrations
{
    /// <inheritdoc />
    public partial class ReCreateReportTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    AdminID = table.Column<int>(nullable: false),
                    CourseID = table.Column<int>(nullable: false),

                    ContentType = table.Column<string>(nullable: false),
                    Data = table.Column<byte[]>(nullable: false),

                    FileName = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false),

                    Type = table.Column<string>(nullable: false),
                    Status = table.Column<string>(nullable: false),

                    GeneratedBy = table.Column<string>(nullable: false),
                    GeneratedDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.ID);

                    table.ForeignKey(
                        name: "FK_Reports_Admins_AdminID",
                        column: x => x.AdminID,
                        principalTable: "Admins",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_Reports_Courses_CourseID",
                        column: x => x.CourseID,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AdminID",
                table: "Reports",
                column: "AdminID");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CourseID",
                table: "Reports",
                column: "CourseID");
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reports");
        }
    }
}
