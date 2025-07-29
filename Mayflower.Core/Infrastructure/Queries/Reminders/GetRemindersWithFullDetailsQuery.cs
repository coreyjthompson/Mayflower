using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Queries.Reminders
{
    public class GetRemindersWithFullDetailsQuery : IQuery<List<Reminder>>
    {
        public GetRemindersWithFullDetailsQuery() { }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = string.Format("GetAllRemindersWithFinancialAccountsQuery", ToString()), SlidingExpiration = TimeSpan.FromMinutes(0) };
    }

    public class GetRemindersWithFullDetailsQueryHandler : IQueryHandler<GetRemindersWithFullDetailsQuery, List<Reminder>>
    {
        private readonly MayflowerContext _db;

        public GetRemindersWithFullDetailsQueryHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<List<Reminder>> HandleAsync(GetRemindersWithFullDetailsQuery query)
        {
            var reminders = await _db.Reminders
                .Include(r => r.TransactionFromAccount)
                .Include(r => r.TransactionToAccount)
                .Include(r => r.Occurrences)
                .ToListAsync();

            return reminders;
        }
    }
}
