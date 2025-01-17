using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FullCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 0,
                column: "Name",
                value: "One-time payment");

            migrationBuilder.UpdateData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 1,
                column: "Value",
                value: "Day");

            migrationBuilder.UpdateData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 2,
                column: "Value",
                value: "Week");

            migrationBuilder.UpdateData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 3,
                column: "Value",
                value: "Month");

            migrationBuilder.InsertData(
                table: "RecurrenceTheme",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[] { 4, "Yearly", "Year" });

            migrationBuilder.UpdateData(
                table: "ReminderTheme",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Bill");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 0,
                column: "Name",
                value: "No Recurrence");

            migrationBuilder.UpdateData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 1,
                column: "Value",
                value: "Daily");

            migrationBuilder.UpdateData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 2,
                column: "Value",
                value: "Weekly");

            migrationBuilder.UpdateData(
                table: "RecurrenceTheme",
                keyColumn: "Id",
                keyValue: 3,
                column: "Value",
                value: "Monthly");

            migrationBuilder.UpdateData(
                table: "ReminderTheme",
                keyColumn: "Id",
                keyValue: 1,
                column: "Name",
                value: "Bill Payment");
        }
    }
}
