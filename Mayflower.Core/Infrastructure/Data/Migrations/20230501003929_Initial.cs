using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    WhenOccurs = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    RecurrenceOrdinalId = table.Column<int>(type: "int", nullable: false)
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
                columns: new[] { "Id", "Amount", "Description", "RecurrenceDayOfMonth", "RecurrenceDayOfWeekId", "RecurrenceInterval", "RecurrenceOrdinalId", "RecurrenceThemeId", "ReminderThemeId", "TransactionFromAccountId", "TransactionToAccountId", "WhenExpires", "WhenOccurs" },
                values: new object[,]
                {
                    { 1, 645.33m, "Lakeview Mortgage Payment", 10, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 400m, "Old National Car Payment", 7, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 120m, "Synchrony/CareCredit Vet Card", 2, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 2, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 71m, "Astound Broadband Internet", 24, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 24, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 12.95m, "Walmart +", 8, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 1.99m, "Google Cloud Storage", 7, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 27.81m, "Pretty Litter", null, 5, 3, 0, 2, 1, 2, null, null, new DateTime(2023, 4, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 19.99m, "Netflix", 7, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 7, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, 9.99m, "Paramount +", 10, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, 60m, "Google Fi Mobile Phone", 15, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, 15.99m, "HBO Max", 16, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 16, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, 15.99m, "Amazon Music", 17, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 17, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, 152.08m, "EnerBank/Regions HVAC Payment", 25, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, 4.99m, "Peacock", 26, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, 10.99m, "Disney +", 26, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 26, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 16, 205.89m, "Nelnet Student Loan", 28, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 17, 100m, "Chandler Water", 20, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 18, 250m, "Centerpoint Gas & Electric", 28, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 5, 28, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 19, 50m, "Cricket Wireless", 30, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 20, 91.50m, "Root Car Insurance", 30, null, 1, 0, 3, 1, 2, null, null, new DateTime(2023, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 21, 95.00m, "Waste Management Trash", 30, null, 3, 0, 3, 1, 2, null, null, new DateTime(2023, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminder");

            migrationBuilder.DropTable(
                name: "TransactionTheme");

            migrationBuilder.DropTable(
                name: "FinancialAccount");

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
