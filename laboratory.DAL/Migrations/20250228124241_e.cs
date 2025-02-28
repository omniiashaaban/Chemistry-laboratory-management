using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laboratory.DAL.Migrations
{
    /// <inheritdoc />
    public partial class e : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NationalId",
                table: "Doctors",
                type: "int",
                maxLength: 14,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NationalId",
                table: "Doctors");
        }
    }
}
