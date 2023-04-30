using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mayflower.Core.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreation : Migration
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
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecurrenceTheme", x => x.Id);
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
                    NickName = table.Column<string>(type: "varchar(255)", nullable: true),
                    FinancialInstitutionId = table.Column<int>(type: "int", nullable: false)
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
                name: "Reminder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WhenToStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReminderThemeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TransactionToAccountId = table.Column<int>(type: "int", nullable: true),
                    TransactionFromAccountId = table.Column<int>(type: "int", nullable: true),
                    RecurrenceThemeId = table.Column<int>(type: "int", nullable: false)
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
                table: "FinancialInstitution",
                columns: new[] { "Id", "Name", "NickName" },
                values: new object[] { 1, "Ally Bank", "Ally" });

            migrationBuilder.InsertData(
                table: "RecurrenceDayOfWeek",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "None", "None" },
                    { 1, "Sunday", "Sunday" },
                    { 2, "Monday", "Monday" },
                    { 4, "Tuesday", "Tuesday" },
                    { 8, "Wednesday", "Wednesday" },
                    { 16, "Thursday", "Thursday" },
                    { 32, "Friday", "Friday" },
                    { 64, "Saturday", "Saturday" }
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
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "No Recurrence", "NoRecurrence" },
                    { 1, "Daily", "Daily" },
                    { 2, "Weekly", "Weekly" },
                    { 3, "Monthly", "Monthly" }
                });

            migrationBuilder.InsertData(
                table: "ReminderTheme",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "Error", "Error" },
                    { 1, "Bill Payment", "Bill" },
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

            migrationBuilder.InsertData(
                table: "FinancialAccount",
                columns: new[] { "Id", "FinancialAccountThemeId", "FinancialInstitutionId", "NickName", "Number" },
                values: new object[,]
                {
                    { 1, 2, 1, "Primary Checking", "1032497347" },
                    { 2, 2, 1, "Bill Pay Account", "1054452618" },
                    { 3, 1, 1, "Primary Savings", "2132880598" },
                    { 4, 1, 1, "Xmas Fund", "2133086542" },
                    { 5, 1, 1, "Me-want Fund", "2141652087" },
                    { 6, 1, 1, "Birthday Fund", "2144275472" },
                    { 7, 1, 1, "Graces Emergency Fund", "2144275977" },
                    { 8, 1, 1, "Landscaping Fund", "2148132695" }
                });

            migrationBuilder.InsertData(
                table: "Reminder",
                columns: new[] { "Id", "Amount", "Description", "RecurrenceThemeId", "ReminderThemeId", "TransactionFromAccountId", "TransactionToAccountId", "WhenToStart" },
                values: new object[,]
                {
                    { 1, 2800.00m, "iMedia paycheck deposit", 0, 2, null, 1, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 2800.00m, "iMedia paycheck deposit", 0, 2, null, 1, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 76.25m, "water bill", 0, 1, 1, null, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 650.00m, "mortage payment", 0, 1, 1, null, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 225.00m, "vectren bill", 0, 1, 1, null, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 235.56m, "car payment", 0, 1, 1, null, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 18.98m, "Netflix", 0, 1, 1, null, new DateTime(2023, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 50m, "Fuel", 0, 1, 1, null, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 50m, "Fuel", 0, 1, 1, null, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 100m, "Cigarettes", 0, 1, 1, null, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 100m, "Cigarettes", 0, 1, 1, null, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 120m, "Smoke", 0, 1, 1, null, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 120m, "Smoke", 0, 1, 1, null, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 90m, "Child support", 0, 1, 1, null, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 90m, "Child support", 0, 1, 1, null, new DateTime(2023, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 90m, "Child support", 0, 1, 1, null, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 90m, "Child support", 0, 1, 1, null, new DateTime(2023, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 2.95m, "Child support fee", 0, 1, 1, null, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 2.95m, "Child support fee", 0, 1, 1, null, new DateTime(2023, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 2.95m, "Child support fee", 0, 1, 1, null, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 2.95m, "Child support fee", 0, 1, 1, null, new DateTime(2023, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 22, 1200.00m, "Transfer from Primary Savings|Transfer to Primary Checking", 0, 3, 3, 1, new DateTime(2023, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 23, 100.00m, "Transfer from Primary Checking|Transfer to Xmas Fund", 0, 3, 1, 4, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 24, 1200.00m, "Transfer from Primary Checking|Transfer to Grace's Emergency Fund", 0, 3, 1, 7, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecurrenceDayOfWeek");

            migrationBuilder.DropTable(
                name: "RecurrenceOrdinal");

            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.DropTable(
                name: "TransactionTheme");

            migrationBuilder.DropTable(
                name: "FinancialAccount");

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
