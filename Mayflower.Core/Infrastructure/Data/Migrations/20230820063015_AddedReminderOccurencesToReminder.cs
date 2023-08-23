using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedReminderOccurencesToReminder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReminderOccurence");

            migrationBuilder.DropTable(
                name: "ReminderOccurenceReason");

            migrationBuilder.CreateTable(
                name: "ReminderOccurrenceReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderOccurrenceReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReminderOccurrence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OccurrenceReasonId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ReplacedById = table.Column<int>(type: "int", nullable: true),
                    WhenReminderScheduledToOccur = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhenCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    _occurenceReasonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderOccurrence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReminderOccurrence_ReminderOccurrenceReason__occurenceReasonId",
                        column: x => x._occurenceReasonId,
                        principalTable: "ReminderOccurrenceReason",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReminderOccurrence_Reminder_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Reminder",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReminderOccurrence_Reminder_ReplacedById",
                        column: x => x.ReplacedById,
                        principalTable: "Reminder",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ReminderOccurrenceReason",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "Unknown", "Unknown" },
                    { 1, "Skipped", "Skipped" },
                    { 2, "Completion", "Complete" },
                    { 3, "Replacement", "Replaced" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurrence__occurenceReasonId",
                table: "ReminderOccurrence",
                column: "_occurenceReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurrence_ParentId",
                table: "ReminderOccurrence",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurrence_ReplacedById",
                table: "ReminderOccurrence",
                column: "ReplacedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReminderOccurrence");

            migrationBuilder.DropTable(
                name: "ReminderOccurrenceReason");

            migrationBuilder.CreateTable(
                name: "ReminderOccurenceReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderOccurenceReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReminderOccurence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActionId = table.Column<int>(type: "int", nullable: false),
                    ParentId = table.Column<int>(type: "int", nullable: false),
                    ReplacedById = table.Column<int>(type: "int", nullable: true),
                    WhenOccurred = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderOccurence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReminderOccurence_ReminderOccurenceReason_ActionId",
                        column: x => x.ActionId,
                        principalTable: "ReminderOccurenceReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReminderOccurence_Reminder_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Reminder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReminderOccurence_Reminder_ReplacedById",
                        column: x => x.ReplacedById,
                        principalTable: "Reminder",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ReminderOccurenceReason",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "None", "None" },
                    { 1, "Skipped", "Skipped" },
                    { 2, "Completion", "Complete" },
                    { 3, "Replacement", "Replaced" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurence_ActionId",
                table: "ReminderOccurence",
                column: "ActionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurence_ParentId",
                table: "ReminderOccurence",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurence_ReplacedById",
                table: "ReminderOccurence",
                column: "ReplacedById");
        }
    }
}
