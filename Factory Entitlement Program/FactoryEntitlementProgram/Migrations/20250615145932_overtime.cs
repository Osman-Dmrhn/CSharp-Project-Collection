using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryEntitlementProgram.Migrations
{
    /// <inheritdoc />
    public partial class overtime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "OvertimeHours",
                table: "WorkDays",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OvertimeHours",
                table: "WorkDays");
        }
    }
}
