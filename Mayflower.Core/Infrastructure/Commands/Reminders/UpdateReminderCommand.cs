using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Commands.Reminders
{
    public class UpdateReminderCommand : ICommand<bool>
    {
        public UpdateReminderCommand() { }

        public int Id { get; set; }
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

    public class UpdateReminderCommandHandler : ICommandHandler<UpdateReminderCommand, bool>
    {
        private readonly MayflowerContext _db;

        public UpdateReminderCommandHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<bool> HandleAsync(UpdateReminderCommand command)
        {
            var reminder = new Reminder();

            if (command.Id > 0)
            {
                reminder = _db.Reminders?.FirstOrDefault(r => r.Id == command.Id) ?? new Reminder();

                if (reminder.Id == 0)
                {
                    return false;
                }

                reminder.WhenBegins = command.WhenBegins;
                reminder.WhenExpires = command.WhenExpires;
                reminder.Amount = command.Amount;
                reminder.ReminderTheme = command.ReminderTheme;
                reminder.Description = command.Description;
                reminder.TransactionToAccountId = command.TransactionToAccountId;
                reminder.TransactionFromAccountId = command.TransactionFromAccountId;
                reminder.RecurrenceTheme = command.RecurrenceTheme;
                reminder.RecurrenceInterval = command.RecurrenceInterval;
                reminder.RecurrenceDayOfMonth = command.RecurrenceDayOfMonth;
                reminder.RecurrenceDayOfWeek = command.RecurrenceDayOfWeek;
                reminder.RecurrenceOrdinal = command.RecurrenceOrdinal;
            }

            await _db.SaveChangesAsync();

            if (reminder.Id != 0)
            {
                return true;
            }

            return false;
        }

    }
}
