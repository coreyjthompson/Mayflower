using Mayflower.Core.DomainModels;
using Mayflower.Core.Extensions;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Commands.ReminderOccurences
{
    public class EditReminderOccurenceCommand : ICommand<bool>
    {
        public EditReminderOccurenceCommand() { }

        public int Id { get; set; }

        public DateOnly WhenOriginallyScheduledToOccur { get; set; }

        public DateOnly? WhenRescheduledToOccur { get; set; }

        public decimal? Amount { get; set; }

        public ReminderOccurrenceCause ReasonForOccurrence { get; set; }

        public override string ToString()
        {
            return string.Format("[Id={0}, WhenOriginallyScheduledToOccur={1}, WhenRescheduledToOccur={2}, Amount={3}, ReasonForOccurrence={4}]", Id, WhenOriginallyScheduledToOccur,
                WhenRescheduledToOccur, Amount, ReasonForOccurrence.ToDescription());
        }
    }

    public class EditReminderOccurenceCommandHandler : ICommandHandler<EditReminderOccurenceCommand, bool>
    {
        private readonly MayflowerContext _db;

        public EditReminderOccurenceCommandHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<bool> HandleAsync(EditReminderOccurenceCommand command)
        {
            if (command.Id == 0)
            {
                return false;
            }

            var entity = _db.ReminderOccurrences.FirstOrDefault(o => o.Id == command.Id);
            if (entity != null)
            {
                entity.ReasonForOccurrence = command.ReasonForOccurrence;
                entity.WhenCreated = DateTimeOffset.Now;
                entity.WhenOriginallyScheduledToOccur = command.WhenOriginallyScheduledToOccur;
                entity.WhenRescheduledToOccur = command.WhenRescheduledToOccur;
                entity.Amount = command.Amount;

                await _db.SaveChangesAsync();

                if (entity.Id != 0)
                {
                    return true;
                }
            }

            return false;
        }

    }
}
