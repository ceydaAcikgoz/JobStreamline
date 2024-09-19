using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JobStreamline.Api.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    CompanyName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    JobPostingLimit = table.Column<int>(type: "integer", nullable: false, defaultValue: 2),
                    ContactEmail = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    NumberOfEmployees = table.Column<int>(type: "integer", nullable: false),
                    CompanySize = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Position = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "CURRENT_DATE + INTERVAL '15 day'"),
                    QualityScore = table.Column<int>(type: "integer", nullable: false),
                    Benefits = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    WorkType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Salary = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Currency = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false, defaultValue: "Active"),
                    Qualifications = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_PhoneNumber",
                table: "Companies",
                column: "PhoneNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_CompanyId",
                table: "Jobs",
                column: "CompanyId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jobs");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
