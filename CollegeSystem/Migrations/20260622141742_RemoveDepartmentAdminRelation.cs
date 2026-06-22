using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollegeSystem.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDepartmentAdminRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Departments_Admins_AdminID",
                table: "Departments");

            migrationBuilder.DropIndex(
                name: "IX_Departments_AdminID",
                table: "Departments");

            migrationBuilder.DropColumn(
                name: "AdminID",
                table: "Departments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdminID",
                table: "Departments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Departments_AdminID",
                table: "Departments",
                column: "AdminID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Departments_Admins_AdminID",
                table: "Departments",
                column: "AdminID",
                principalTable: "Admins",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
