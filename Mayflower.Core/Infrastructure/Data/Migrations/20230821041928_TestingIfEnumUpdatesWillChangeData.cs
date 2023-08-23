using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class TestingIfEnumUpdatesWillChangeData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReminderOccurrenceReason",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Value" },
                values: new object[] { "Skip", "Skip" });

            migrationBuilder.UpdateData(
                table: "ReminderOccurrenceReason",
                keyColumn: "Id",
                keyValue: 2,
                column: "Value",
                value: "Completion");

            migrationBuilder.UpdateData(
                table: "ReminderOccurrenceReason",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "Value" },
                values: new object[] { "Edit", "Edit" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "ReminderOccurrenceReason",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Name", "Value" },
                values: new object[] { "Skipped", "Skipped" });

            migrationBuilder.UpdateData(
                table: "ReminderOccurrenceReason",
                keyColumn: "Id",
                keyValue: 2,
                column: "Value",
                value: "Complete");

            migrationBuilder.UpdateData(
                table: "ReminderOccurrenceReason",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "Value" },
                values: new object[] { "Replacement", "Replaced" });
        }
    }
}
