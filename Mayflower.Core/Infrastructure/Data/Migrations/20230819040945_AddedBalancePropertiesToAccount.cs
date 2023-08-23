using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedBalancePropertiesToAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AvailableBalance",
                table: "FinancialAccount",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "LedgerBalance",
                table: "FinancialAccount",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WhenLastUpdated",
                table: "FinancialAccount",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "FinancialAccount",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AvailableBalance", "LedgerBalance", "WhenLastUpdated" },
                values: new object[] { 0m, 0m, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "FinancialAccount",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AvailableBalance", "LedgerBalance", "WhenLastUpdated" },
                values: new object[] { 0m, 0m, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "FinancialAccount",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AvailableBalance", "LedgerBalance", "WhenLastUpdated" },
                values: new object[] { 0m, 0m, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "FinancialAccount",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AvailableBalance", "LedgerBalance", "WhenLastUpdated" },
                values: new object[] { 0m, 0m, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "FinancialAccount",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AvailableBalance", "LedgerBalance", "WhenLastUpdated" },
                values: new object[] { 0m, 0m, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "FinancialAccount",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "AvailableBalance", "LedgerBalance", "WhenLastUpdated" },
                values: new object[] { 0m, 0m, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "FinancialAccount",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "AvailableBalance", "LedgerBalance", "WhenLastUpdated" },
                values: new object[] { 0m, 0m, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "FinancialAccount",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "AvailableBalance", "LedgerBalance", "WhenLastUpdated" },
                values: new object[] { 0m, 0m, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableBalance",
                table: "FinancialAccount");

            migrationBuilder.DropColumn(
                name: "LedgerBalance",
                table: "FinancialAccount");

            migrationBuilder.DropColumn(
                name: "WhenLastUpdated",
                table: "FinancialAccount");
        }
    }
}
