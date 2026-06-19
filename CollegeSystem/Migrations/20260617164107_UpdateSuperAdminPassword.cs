using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSuperAdminPassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SuperAdmins",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "AQAAAAIAAYagAAAAEAC0BU6coyUVwufnT74u5I+OZmeZ6+RAZH6D2ngNXsQCmE6ZgaY5SRftfNpTP3iq9A==");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "SuperAdmins",
                keyColumn: "ID",
                keyValue: 1,
                column: "Password",
                value: "Super@1234");
        }
    }
}
