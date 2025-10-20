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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Surname = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Role = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    VerificationToken = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Verified = table.Column<DateTime>(type: "datetime2", maxLength: 150, nullable: true),
                    ResetToken = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ResetTokenExpires = table.Column<DateTime>(type: "datetime", nullable: true),
                    PasswordReset = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_Department", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_ImportEmployeeHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_ImportEmployeeHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Expires = table.Column<DateTime>(type: "datetime", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime", nullable: false),
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Surname = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    HireDate = table.Column<DateTime>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Photo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateOnly>(type: "date", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImportEmployeeHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_Employee", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_Employee_WOLDS_HR_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "WOLDS_HR_Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_Employee_WOLDS_HR_ImportEmployeeHistory_ImportEmployeeHistoryId",
                        column: x => x.ImportEmployeeHistoryId,
                        principalTable: "WOLDS_HR_ImportEmployeeHistory",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_ImportEmployeeExistingHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Surname = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Created = table.Column<DateOnly>(type: "date", nullable: false),
                    ImportEmployeeHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeImportHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_ImportEmployeeExistingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_ImportEmployeeExistingHistory_WOLDS_HR_ImportEmployeeHistory_EmployeeImportHistoryId",
                        column: x => x.EmployeeImportHistoryId,
                        principalTable: "WOLDS_HR_ImportEmployeeHistory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_ImportEmployeeExistingHistory_WOLDS_HR_ImportEmployeeHistory_ImportEmployeeHistoryId",
                        column: x => x.ImportEmployeeHistoryId,
                        principalTable: "WOLDS_HR_ImportEmployeeHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_ImportEmployeeFailedHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Employee = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ImportEmployeeHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_ImportEmployeeFailedHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_ImportEmployeeFailedHistory_WOLDS_HR_ImportEmployeeHistory_ImportEmployeeHistoryId",
                        column: x => x.ImportEmployeeHistoryId,
                        principalTable: "WOLDS_HR_ImportEmployeeHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WOLDS_HR_ImportEmployeeFailedErrorHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImportEmployeeFailedHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WOLDS_HR_ImportEmployeeFailedErrorHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WOLDS_HR_ImportEmployeeFailedErrorHistory_WOLDS_HR_ImportEmployeeFailedHistory_ImportEmployeeFailedHistoryId",
                        column: x => x.ImportEmployeeFailedHistoryId,
                        principalTable: "WOLDS_HR_ImportEmployeeFailedHistory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_Employee_DepartmentId",
                table: "WOLDS_HR_Employee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_Employee_ImportEmployeeHistoryId",
                table: "WOLDS_HR_Employee",
                column: "ImportEmployeeHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_ImportEmployeeExistingHistory_EmployeeImportHistoryId",
                table: "WOLDS_HR_ImportEmployeeExistingHistory",
                column: "EmployeeImportHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_ImportEmployeeExistingHistory_ImportEmployeeHistoryId",
                table: "WOLDS_HR_ImportEmployeeExistingHistory",
                column: "ImportEmployeeHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_ImportEmployeeFailedErrorHistory_ImportEmployeeFailedHistoryId",
                table: "WOLDS_HR_ImportEmployeeFailedErrorHistory",
                column: "ImportEmployeeFailedHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WOLDS_HR_ImportEmployeeFailedHistory_ImportEmployeeHistoryId",
                table: "WOLDS_HR_ImportEmployeeFailedHistory",
                column: "ImportEmployeeHistoryId");

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
                name: "WOLDS_HR_ImportEmployeeExistingHistory");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_ImportEmployeeFailedErrorHistory");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_RefreshToken");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_Department");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_ImportEmployeeFailedHistory");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_Account");

            migrationBuilder.DropTable(
                name: "WOLDS_HR_ImportEmployeeHistory");
        }
    }
}
