using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Queries.Reminders
{
    public class GetReminders : IQuery<List<Reminder>>
    {

    }

    public class GetRemindersQueryHandler : IQueryHandler<GetReminders, List<Reminder>>
    {
        private readonly MayflowerContext _db;

        public GetRemindersQueryHandler(MayflowerContext db)
        {
            _db = db;
        }

        public async Task<List<Reminder>> HandleAsync(GetReminders query)
        {
            return await _db.Reminders.OrderBy(x => x.Id).ToListAsync();
        }

    }
}
