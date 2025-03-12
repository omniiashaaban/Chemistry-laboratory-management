using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace laboratory.DAL.Migrations
{
    /// <inheritdoc />
    public partial class t : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Students",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Specialty",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "LabAdminId",
                table: "Materials",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Doctors",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "AppUser",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabAdmin",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Specialty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LabAdmin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LabAdmin_AppUser_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AppUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Students_AppUserId",
                table: "Students",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materials_LabAdminId",
                table: "Materials",
                column: "LabAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_AppUserId",
                table: "Doctors",
                column: "AppUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LabAdmin_AppUserId",
                table: "LabAdmin",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_AppUser_AppUserId",
                table: "Doctors",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materials_LabAdmin_LabAdminId",
                table: "Materials",
                column: "LabAdminId",
                principalTable: "LabAdmin",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AppUser_AppUserId",
                table: "Students",
                column: "AppUserId",
                principalTable: "AppUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_AppUser_AppUserId",
                table: "Doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_Materials_LabAdmin_LabAdminId",
                table: "Materials");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AppUser_AppUserId",
                table: "Students");

            migrationBuilder.DropTable(
                name: "LabAdmin");

            migrationBuilder.DropTable(
                name: "AppUser");

            migrationBuilder.DropIndex(
                name: "IX_Students_AppUserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_Materials_LabAdminId",
                table: "Materials");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_AppUserId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "Specialty",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "LabAdminId",
                table: "Materials");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Doctors");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "Students",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
