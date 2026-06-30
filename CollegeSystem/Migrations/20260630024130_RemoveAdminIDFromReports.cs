using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAdminIDFromReports : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Admins_AdminID",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "AdminID",
                table: "Reports",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Admins_AdminID",
                table: "Reports",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Admins_AdminID",
                table: "Reports");

            migrationBuilder.AlterColumn<int>(
                name: "AdminID",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Admins_AdminID",
                table: "Reports",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
