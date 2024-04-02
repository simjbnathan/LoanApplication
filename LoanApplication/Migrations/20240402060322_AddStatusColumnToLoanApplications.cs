using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusColumnToLoanApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "LoanApplications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "LoanApplications");
        }
    }
}
