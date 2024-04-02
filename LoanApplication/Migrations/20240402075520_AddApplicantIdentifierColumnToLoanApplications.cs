﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplicationApi.Migrations
{
    /// <inheritdoc />
    public partial class AddApplicantIdentifierColumnToLoanApplications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicantIdentifier",
                table: "LoanApplications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "LoanApplications",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApplicantIdentifier",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "LoanApplications");
        }
    }
}
