using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Queries.Reminders
{
    public class GetAllReminders : IQuery<List<Reminder>>
    {

    }

    public class GetAllRemindersQueryHandler : IQueryHandler<GetAllReminders, List<Reminder>>
    {
        private readonly MayflowerContext _db;

        public GetAllRemindersQueryHandler(MayflowerContext db)
        {
            _db = db;
        }

        public async Task<List<Reminder>> HandleAsync(GetAllReminders query)
        {
            return await _db.Reminders.OrderBy(x => x.Id).ToListAsync();
        }

    }
}
