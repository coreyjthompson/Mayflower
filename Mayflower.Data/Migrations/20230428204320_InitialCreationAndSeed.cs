using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mayflower.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreationAndSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "TransactionAccountTheme",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "varchar(25)", nullable: false),
                    Name = table.Column<string>(type: "varchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionAccountTheme", x => x.Id);
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
                name: "TransactionAccount",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<string>(type: "varchar(255)", nullable: false),
                    TransactionAccountThemeId = table.Column<int>(type: "int", nullable: false),
                    NickName = table.Column<string>(type: "varchar(255)", nullable: true),
                    AccountVendor = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionAccount", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionAccount_TransactionAccountTheme_TransactionAccountThemeId",
                        column: x => x.TransactionAccountThemeId,
                        principalTable: "TransactionAccountTheme",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reminder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionAccountId = table.Column<int>(type: "int", nullable: false),
                    WhenToStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    ReminderThemeId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayFrom = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RecurrenceThemeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminder", x => x.Id);
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
                    table.ForeignKey(
                        name: "FK_Reminder_TransactionAccount_TransactionAccountId",
                        column: x => x.TransactionAccountId,
                        principalTable: "TransactionAccount",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    { 0, "Unknown", "Unknown" },
                    { 1, "Bill", "Bill" },
                    { 2, "Transfer", "Transfer" },
                    { 3, "Income", "Income" }
                });

            migrationBuilder.InsertData(
                table: "TransactionAccountTheme",
                columns: new[] { "Id", "Name", "Value" },
                values: new object[,]
                {
                    { 0, "None", "None" },
                    { 1, "Savings", "Savings" },
                    { 2, "Checking", "Checking" },
                    { 3, "Marketing", "Marketing" }
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
                table: "TransactionAccount",
                columns: new[] { "Id", "AccountVendor", "NickName", "Number", "TransactionAccountThemeId" },
                values: new object[,]
                {
                    { 1, "Ally Bank", "Primary Checking", "1032497347", 2 },
                    { 2, "Ally Bank", "Bill Pay Account", "1054452618", 2 },
                    { 3, "Ally Bank", "Primary Savings", "2132880598", 1 },
                    { 4, "Ally Bank", "Xmas Fund", "2133086542", 1 },
                    { 5, "Ally Bank", "Me-want Fund", "2141652087", 1 },
                    { 6, "Ally Bank", "Birthday Fund", "2144275472", 1 },
                    { 7, "Ally Bank", "Graces Emergency Fund", "2144275977", 1 },
                    { 8, "Ally Bank", "Landscaping Fund", "2148132695", 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_RecurrenceThemeId",
                table: "Reminder",
                column: "RecurrenceThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_ReminderThemeId",
                table: "Reminder",
                column: "ReminderThemeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reminder_TransactionAccountId",
                table: "Reminder",
                column: "TransactionAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionAccount_TransactionAccountThemeId",
                table: "TransactionAccount",
                column: "TransactionAccountThemeId");
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
                name: "RecurrenceTheme");

            migrationBuilder.DropTable(
                name: "ReminderTheme");

            migrationBuilder.DropTable(
                name: "TransactionAccount");

            migrationBuilder.DropTable(
                name: "TransactionAccountTheme");
        }
    }
}
