using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laboratory.DAL.Migrations
{
    /// <inheritdoc />
    public partial class pp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "DepartmentName",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentName",
                table: "Doctors");

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "Doctors",
                type: "nvarchar(14)",
                maxLength: 14,
                nullable: false,
                defaultValue: "");
        }
    }
}
