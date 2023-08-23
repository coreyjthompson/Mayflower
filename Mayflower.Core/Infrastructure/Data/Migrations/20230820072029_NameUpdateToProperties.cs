using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class NameUpdateToProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReminderOccurrence_ReminderOccurrenceReason__occurenceReasonId",
                table: "ReminderOccurrence");

            migrationBuilder.DropIndex(
                name: "IX_ReminderOccurrence__occurenceReasonId",
                table: "ReminderOccurrence");

            migrationBuilder.DropColumn(
                name: "_occurenceReasonId",
                table: "ReminderOccurrence");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurrence_OccurrenceReasonId",
                table: "ReminderOccurrence",
                column: "OccurrenceReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderOccurrence_ReminderOccurrenceReason_OccurrenceReasonId",
                table: "ReminderOccurrence",
                column: "OccurrenceReasonId",
                principalTable: "ReminderOccurrenceReason",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReminderOccurrence_ReminderOccurrenceReason_OccurrenceReasonId",
                table: "ReminderOccurrence");

            migrationBuilder.DropIndex(
                name: "IX_ReminderOccurrence_OccurrenceReasonId",
                table: "ReminderOccurrence");

            migrationBuilder.AddColumn<int>(
                name: "_occurenceReasonId",
                table: "ReminderOccurrence",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurrence__occurenceReasonId",
                table: "ReminderOccurrence",
                column: "_occurenceReasonId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReminderOccurrence_ReminderOccurrenceReason__occurenceReasonId",
                table: "ReminderOccurrence",
                column: "_occurenceReasonId",
                principalTable: "ReminderOccurrenceReason",
                principalColumn: "Id");
        }
    }
}
