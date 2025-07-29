using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Commands.Reminders
{
    public class InsertReminderCommand : ICommand<bool>
    {
        public InsertReminderCommand() { }

        public DateOnly WhenBegins { get; set; }
        public DateOnly? WhenExpires { get; set; }
        public decimal Amount { get; set; }
        public ReminderStyle ReminderTheme { get; set; }
        public string? Description { get; set; }
        public int? TransactionToAccountId { get; set; }
        public int? TransactionFromAccountId { get; set; }
        public RecurrenceStyle RecurrenceTheme { get; set; }
        public int RecurrenceInterval { get; set; }
        public int? RecurrenceDayOfMonth { get; set; }
        public DayOfWeek? RecurrenceDayOfWeek { get; set; }
        public RecurrencePosition RecurrenceOrdinal { get; set; }
    }

    public class InsertReminderCommandHandler : ICommandHandler<InsertReminderCommand, bool>
    {
        private readonly MayflowerContext _db;

        public InsertReminderCommandHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<bool> HandleAsync(InsertReminderCommand command)
        {
            Reminder reminder;

            reminder = new Reminder
            {
                WhenBegins = command.WhenBegins,
                WhenExpires = command.WhenExpires,
                Amount = command.Amount,
                ReminderTheme = command.ReminderTheme,
                Description = command.Description,
                TransactionToAccountId = command.TransactionToAccountId,
                TransactionFromAccountId = command.TransactionFromAccountId,
                RecurrenceTheme = command.RecurrenceTheme,
                RecurrenceInterval = command.RecurrenceInterval,
                RecurrenceDayOfMonth = command.RecurrenceDayOfMonth,
                RecurrenceDayOfWeek = command.RecurrenceDayOfWeek,
                RecurrenceOrdinal = command.RecurrenceOrdinal
            };

            await _db.Reminders.AddAsync(reminder);
            await _db.SaveChangesAsync();

            if (reminder.Id != 0)
            {
                return true;
            }

            return false;
        }

    }
}
