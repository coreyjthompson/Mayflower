using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedReminderHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NickName",
                table: "FinancialAccount",
                newName: "Nickname");

            migrationBuilder.AddColumn<int>(
                name: "InactiveReasonId",
                table: "Reminder",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "InactiveReminderReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InactiveReminderReason", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReminderOccurenceReason",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
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
                table: "InactiveReminderReason",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "Unknown", "Unknown" },
                    { 1, "Completion", "Skipped" },
                    { 2, "Deleted", "Complete" }
                });

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 1,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 2,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 3,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 4,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 5,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 6,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 7,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 8,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 9,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 10,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 11,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 12,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 13,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 14,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 15,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 16,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 17,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 18,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 19,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 20,
                column: "InactiveReasonId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Reminder",
                keyColumn: "Id",
                keyValue: 21,
                column: "InactiveReasonId",
                value: null);

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
                name: "IX_Reminder_InactiveReasonId",
                table: "Reminder",
                column: "InactiveReasonId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Reminder_InactiveReminderReason_InactiveReasonId",
                table: "Reminder",
                column: "InactiveReasonId",
                principalTable: "InactiveReminderReason",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reminder_InactiveReminderReason_InactiveReasonId",
                table: "Reminder");

            migrationBuilder.DropTable(
                name: "InactiveReminderReason");

            migrationBuilder.DropTable(
                name: "ReminderOccurence");

            migrationBuilder.DropTable(
                name: "ReminderOccurenceReason");

            migrationBuilder.DropIndex(
                name: "IX_Reminder_InactiveReasonId",
                table: "Reminder");

            migrationBuilder.DropColumn(
                name: "InactiveReasonId",
                table: "Reminder");

            migrationBuilder.RenameColumn(
                name: "Nickname",
                table: "FinancialAccount",
                newName: "NickName");
        }
    }
}
