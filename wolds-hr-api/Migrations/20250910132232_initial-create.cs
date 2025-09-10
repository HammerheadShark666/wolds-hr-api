using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace wolds_hr_api.Migrations
{
    /// <inheritdoc />
    public partial class initialcreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WOLDS_HR_Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcceptTerms = table.Column<bool>(type: "bit", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Verified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PasswordReset = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_Department",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_EmployeeImport",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_EmployeeImport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RevokedByIp = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReplacedByToken = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_RefreshToken_WOLDS_HR_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "WOLDS_HR_Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_Employee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    HireDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateOnly>(type: "date", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeImportId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_Employee_WOLDS_HR_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "WOLDS_HR_Department",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_Employee_WOLDS_HR_EmployeeImport_EmployeeImportId",
                        column: x => x.EmployeeImportId,
                        principalTable: "WOLDS_HR_EmployeeImport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_ExistingEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateOnly>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateOnly>(type: "date", nullable: false),
                    EmployeeImportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_ExistingEmployee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_ExistingEmployee_WOLDS_HR_EmployeeImport_EmployeeImportId",
                        column: x => x.EmployeeImportId,
                        principalTable: "WOLDS_HR_EmployeeImport",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_Employee_DepartmentId",
                table: "WOLDS_HR_Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_Employee_EmployeeImportId",
                table: "WOLDS_HR_Employee",
                column: "EmployeeImportId");

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_ExistingEmployee_EmployeeImportId",
                table: "WOLDS_HR_ExistingEmployee",
                column: "EmployeeImportId");

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_RefreshToken_AccountId",
                table: "WOLDS_HR_RefreshToken",
                column: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WOLDS_HR_Employee");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_ExistingEmployee");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_RefreshToken");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_Department");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_EmployeeImport");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_Account");
        }
    }
}
