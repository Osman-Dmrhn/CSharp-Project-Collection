using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FactoryEntitlementProgram.Migrations
{
    /// <inheritdoc />
    public partial class pay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SaatlikUcret",
                table: "Employees",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaatlikUcret",
                table: "Employees");
        }
    }
}
