using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReminderOccurrenceWithNewDateOnly : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhenOccurred",
                table: "ReminderOccurrence",
                newName: "WhenCreated");

            migrationBuilder.AddColumn<DateTime>(
                name: "WhenRescheduledToOccur",
                table: "ReminderOccurrence",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WhenRescheduledToOccur",
                table: "ReminderOccurrence");

            migrationBuilder.RenameColumn(
                name: "WhenCreated",
                table: "ReminderOccurrence",
                newName: "WhenOccurred");
        }
    }
}
