using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdatesToReminderOccurences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReminderOccurrence_Reminder_ParentId",
                table: "ReminderOccurrence");

            migrationBuilder.DropForeignKey(
                name: "FK_ReminderOccurrence_Reminder_ReplacedById",
                table: "ReminderOccurrence");

            migrationBuilder.DropIndex(
                name: "IX_ReminderOccurrence_ReplacedById",
                table: "ReminderOccurrence");

            migrationBuilder.DropColumn(
                name: "ReplacedById",
                table: "ReminderOccurrence");

            migrationBuilder.RenameColumn(
                name: "WhenReminderScheduledToOccur",
                table: "ReminderOccurrence",
                newName: "WhenOriginallyScheduledToOccur");

            migrationBuilder.RenameColumn(
                name: "WhenCreated",
                table: "ReminderOccurrence",
                newName: "WhenOccurred");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "ReminderOccurrence",
                newName: "ReminderId");

            migrationBuilder.RenameIndex(
                name: "IX_ReminderOccurrence_ParentId",
                table: "ReminderOccurrence",
                newName: "IX_ReminderOccurrence_ReminderId");

            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "ReminderOccurrence",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "ReminderOccurrence",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderOccurrence_Reminder_ReminderId",
                table: "ReminderOccurrence",
                column: "ReminderId",
                principalTable: "Reminder",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReminderOccurrence_Reminder_ReminderId",
                table: "ReminderOccurrence");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "ReminderOccurrence");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "ReminderOccurrence");

            migrationBuilder.RenameColumn(
                name: "WhenOriginallyScheduledToOccur",
                table: "ReminderOccurrence",
                newName: "WhenReminderScheduledToOccur");

            migrationBuilder.RenameColumn(
                name: "WhenOccurred",
                table: "ReminderOccurrence",
                newName: "WhenCreated");

            migrationBuilder.RenameColumn(
                name: "ReminderId",
                table: "ReminderOccurrence",
                newName: "ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_ReminderOccurrence_ReminderId",
                table: "ReminderOccurrence",
                newName: "IX_ReminderOccurrence_ParentId");

            migrationBuilder.AddColumn<int>(
                name: "ReplacedById",
                table: "ReminderOccurrence",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurrence_ReplacedById",
                table: "ReminderOccurrence",
                column: "ReplacedById");

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderOccurrence_Reminder_ParentId",
                table: "ReminderOccurrence",
                column: "ParentId",
                principalTable: "Reminder",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderOccurrence_Reminder_ReplacedById",
                table: "ReminderOccurrence",
                column: "ReplacedById",
                principalTable: "Reminder",
                principalColumn: "Id");
        }
    }
}
