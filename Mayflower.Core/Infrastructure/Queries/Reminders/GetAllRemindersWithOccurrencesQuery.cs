using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Queries.Reminders
{
    public class GetAllRemindersWithOccurrencesQuery : IQuery<List<Reminder>>
    {
        public GetAllRemindersWithOccurrencesQuery() { }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = string.Format("GetAllRemindersWithOccurrencesQuery", ToString()), SlidingExpiration = TimeSpan.FromMinutes(0) };

    }

    public class GetAllRemindersWithOccurrencesQueryHandler : IQueryHandler<GetAllRemindersWithOccurrencesQuery, List<Reminder>>
    {
        private readonly MayflowerContext _db;

        public GetAllRemindersWithOccurrencesQueryHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<List<Reminder>> HandleAsync(GetAllRemindersWithOccurrencesQuery query)
        {
            var reminders = await _db.Reminders
                .Include(r => r.Occurrences)
                .ToListAsync();

            return reminders;
        }
    }
}
