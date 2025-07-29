using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FinancialAccountTheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccountTheme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialInstitution",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    NickName = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialInstitution", x => x.Id);
                });

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
                name: "RecurrenceDayOfWeek",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceDayOfWeek", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurrenceOrdinal",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceOrdinal", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecurrenceTheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false),
                    Description = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceTheme", x => x.Id);
                });

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
                name: "ReminderTheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderTheme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionTheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionTheme", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "varchar(255)", nullable: false),
                    FinancialAccountThemeId = table.Column<int>(type: "int", nullable: false),
                    Nickname = table.Column<string>(type: "varchar(255)", nullable: true),
                    FinancialInstitutionId = table.Column<int>(type: "int", nullable: false),
                    LedgerBalance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    AvailableBalance = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    WhenLastUpdated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAccount_FinancialAccountTheme_FinancialAccountThemeId",
                        column: x => x.FinancialAccountThemeId,
                        principalTable: "FinancialAccountTheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialAccount_FinancialInstitution_FinancialInstitutionId",
                        column: x => x.FinancialInstitutionId,
                        principalTable: "FinancialInstitution",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "varchar(100)", nullable: false),
                    ExternalTransactionId = table.Column<string>(type: "varchar(125)", nullable: false),
                    WhenPosted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    RefNumber = table.Column<string>(type: "varchar(255)", nullable: true),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Memo = table.Column<string>(type: "varchar(255)", nullable: true),
                    FinancialAccountId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialTransaction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialTransaction_FinancialAccount_FinancialAccountId",
                        column: x => x.FinancialAccountId,
                        principalTable: "FinancialAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reminder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhenBegins = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhenExpires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReminderThemeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionToAccountId = table.Column<int>(type: "int", nullable: true),
                    TransactionFromAccountId = table.Column<int>(type: "int", nullable: true),
                    RecurrenceThemeId = table.Column<int>(type: "int", nullable: false),
                    RecurrenceInterval = table.Column<int>(type: "int", nullable: false),
                    RecurrenceDayOfMonth = table.Column<int>(type: "int", nullable: true),
                    RecurrenceDayOfWeekId = table.Column<int>(type: "int", nullable: true),
                    RecurrenceOrdinalId = table.Column<int>(type: "int", nullable: false),
                    InactiveReasonId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminder_FinancialAccount_TransactionFromAccountId",
                        column: x => x.TransactionFromAccountId,
                        principalTable: "FinancialAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reminder_FinancialAccount_TransactionToAccountId",
                        column: x => x.TransactionToAccountId,
                        principalTable: "FinancialAccount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reminder_InactiveReminderReason_InactiveReasonId",
                        column: x => x.InactiveReasonId,
                        principalTable: "InactiveReminderReason",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reminder_RecurrenceDayOfWeek_RecurrenceDayOfWeekId",
                        column: x => x.RecurrenceDayOfWeekId,
                        principalTable: "RecurrenceDayOfWeek",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reminder_RecurrenceOrdinal_RecurrenceOrdinalId",
                        column: x => x.RecurrenceOrdinalId,
                        principalTable: "RecurrenceOrdinal",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reminder_RecurrenceTheme_RecurrenceThemeId",
                        column: x => x.RecurrenceThemeId,
                        principalTable: "RecurrenceTheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reminder_ReminderTheme_ReminderThemeId",
                        column: x => x.ReminderThemeId,
                        principalTable: "ReminderTheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReminderOccurrence",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhenOriginallyScheduledToOccur = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WhenRescheduledToOccur = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OccurrenceReasonId = table.Column<int>(type: "int", nullable: false),
                    ReminderId = table.Column<int>(type: "int", nullable: false),
                    WhenCreated = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReminderOccurrence", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReminderOccurrence_ReminderOccurrenceReason_OccurrenceReasonId",
                        column: x => x.OccurrenceReasonId,
                        principalTable: "ReminderOccurrenceReason",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReminderOccurrence_Reminder_ReminderId",
                        column: x => x.ReminderId,
                        principalTable: "Reminder",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "FinancialAccountTheme",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "None", "None" },
                    { 1, "Savings", "Savings" },
                    { 2, "Checking", "Checking" },
                    { 3, "Marketing", "Marketing" }
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

            migrationBuilder.InsertData(
                table: "RecurrenceDayOfWeek",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "Sunday", "Sunday" },
                    { 1, "Monday", "Monday" },
                    { 2, "Tuesday", "Tuesday" },
                    { 3, "Wednesday", "Wednesday" },
                    { 4, "Thursday", "Thursday" },
                    { 5, "Friday", "Friday" },
                    { 6, "Saturday", "Saturday" }
                });

            migrationBuilder.InsertData(
                table: "RecurrenceOrdinal",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "None", "None" },
                    { 1, "First", "First" },
                    { 2, "Second", "Second" },
                    { 3, "Third", "Third" },
                    { 4, "Fourth", "Fourth" },
                    { 5, "Last", "Last" }
                });

            migrationBuilder.InsertData(
                table: "RecurrenceTheme",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 0, "One-time payment", "NoRecurrence" },
                    { 1, "Daily", "Day" },
                    { 2, "Weekly", "Week" },
                    { 3, "Monthly", "Month" },
                    { 4, "Yearly", "Year" }
                });

            migrationBuilder.InsertData(
                table: "ReminderOccurrenceReason",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "Unknown", "Unknown" },
                    { 1, "Skip", "Skip" },
                    { 2, "Completion", "Completion" },
                    { 3, "Edit", "Edit" }
                });

            migrationBuilder.InsertData(
                table: "ReminderTheme",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "Error", "Error" },
                    { 1, "Bill", "Bill" },
                    { 2, "Income", "Income" },
                    { 3, "Transfer", "Transfer" }
                });

            migrationBuilder.InsertData(
                table: "TransactionTheme",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "None", "None" },
                    { 1, "Withdraw", "Withdraw" },
                    { 2, "Deposit", "Deposit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_FinancialAccountThemeId",
                table: "FinancialAccount",
                column: "FinancialAccountThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAccount_FinancialInstitutionId",
                table: "FinancialAccount",
                column: "FinancialInstitutionId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialTransaction_FinancialAccountId",
                table: "FinancialTransaction",
                column: "FinancialAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_InactiveReasonId",
                table: "Reminder",
                column: "InactiveReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_RecurrenceDayOfWeekId",
                table: "Reminder",
                column: "RecurrenceDayOfWeekId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_RecurrenceOrdinalId",
                table: "Reminder",
                column: "RecurrenceOrdinalId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_RecurrenceThemeId",
                table: "Reminder",
                column: "RecurrenceThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_ReminderThemeId",
                table: "Reminder",
                column: "ReminderThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_TransactionFromAccountId",
                table: "Reminder",
                column: "TransactionFromAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_TransactionToAccountId",
                table: "Reminder",
                column: "TransactionToAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurrence_OccurrenceReasonId",
                table: "ReminderOccurrence",
                column: "OccurrenceReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_ReminderOccurrence_ReminderId",
                table: "ReminderOccurrence",
                column: "ReminderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FinancialTransaction");

            migrationBuilder.DropTable(
                name: "ReminderOccurrence");

            migrationBuilder.DropTable(
                name: "TransactionTheme");

            migrationBuilder.DropTable(
                name: "ReminderOccurrenceReason");

            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.DropTable(
                name: "FinancialAccount");

            migrationBuilder.DropTable(
                name: "InactiveReminderReason");

            migrationBuilder.DropTable(
                name: "RecurrenceDayOfWeek");

            migrationBuilder.DropTable(
                name: "RecurrenceOrdinal");

            migrationBuilder.DropTable(
                name: "RecurrenceTheme");

            migrationBuilder.DropTable(
                name: "ReminderTheme");

            migrationBuilder.DropTable(
                name: "FinancialAccountTheme");

            migrationBuilder.DropTable(
                name: "FinancialInstitution");
        }
    }
}
