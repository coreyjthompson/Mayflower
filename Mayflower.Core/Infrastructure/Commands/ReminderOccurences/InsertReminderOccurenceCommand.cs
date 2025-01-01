using Mayflower.Core.DomainModels;
using Mayflower.Core.Extensions;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Commands.ReminderOccurences
{
    public class InsertReminderOccurenceCommand : ICommand<bool>
    {
        public InsertReminderOccurenceCommand() { }

        public int ReminderId { get; set; }

        public DateOnly WhenOriginallyScheduledToOccur { get; set; }

        public DateOnly? WhenRescheduledToOccur { get; set; }

        public decimal? Amount { get; set; }

        public ReminderOccurrenceCause ReasonForOccurrence { get; set; }

        public override string ToString()
        {
            return string.Format("[ReminderId={0}, WhenOriginallyScheduledToOccur={1}, WhenRescheduledToOccur={2}, Amount={3}, ReasonForOccurrence={4}]", ReminderId, WhenOriginallyScheduledToOccur,
                WhenRescheduledToOccur, Amount, ReasonForOccurrence.ToDescription());
        }
    }

    public class InsertReminderOccurenceCommandHandler : ICommandHandler<InsertReminderOccurenceCommand, bool>
    {
        private readonly MayflowerContext _db;

        public InsertReminderOccurenceCommandHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<bool> HandleAsync(InsertReminderOccurenceCommand command)
        {
            if (command.ReminderId == 0)
            {
                return false;
            }

            var occurrence = new ReminderOccurrence
            {
                ReminderId = command.ReminderId,
                ReasonForOccurrence = command.ReasonForOccurrence,
                WhenCreated = DateTimeOffset.Now,
                WhenOriginallyScheduledToOccur = command.WhenOriginallyScheduledToOccur,
                WhenRescheduledToOccur = command.WhenRescheduledToOccur,
                Amount = command.Amount,
            };

            await _db.ReminderOccurrences.AddAsync(occurrence);
            await _db.SaveChangesAsync();

            if (occurrence.Id != 0)
            {
                return true;
            }

            return false;
        }

    }
}
