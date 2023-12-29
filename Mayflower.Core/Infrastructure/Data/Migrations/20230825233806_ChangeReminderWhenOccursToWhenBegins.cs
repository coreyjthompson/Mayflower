using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mayflower.Core.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeReminderWhenOccursToWhenBegins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhenOccurs",
                table: "Reminder",
                newName: "WhenBegins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WhenBegins",
                table: "Reminder",
                newName: "WhenOccurs");
        }
    }
}
