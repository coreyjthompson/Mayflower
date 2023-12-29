using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Queries.Reminders
{
    public class GetAllRemindersByFinancialAccountIdQuery : IQuery<List<Reminder>>
    {
        public int Id { get; set; }

        public GetAllRemindersByFinancialAccountIdQuery() { }

        public GetAllRemindersByFinancialAccountIdQuery(int id)
        {
            Id = id;
        }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = string.Format("GetAllRemindersByFinancialAccountIdQuery-{0}", ToString()), SlidingExpiration = TimeSpan.FromMinutes(0) };

        public override string ToString()
        {
            return string.Format("[Id={0}]", Id);
        }
    }

    public class GetAllRemindersByFinancialAccountIdQueryHandler : IQueryHandler<GetAllRemindersByFinancialAccountIdQuery, List<Reminder>>
    {
        private readonly MayflowerContext _db;

        public GetAllRemindersByFinancialAccountIdQueryHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<List<Reminder>> HandleAsync(GetAllRemindersByFinancialAccountIdQuery query)
        {
            var reminders = await _db.Reminders
                .Include(r => r.Occurrences)
                .Where(r => r.TransactionFromAccountId == query.Id || r.TransactionToAccountId == query.Id)
                .ToListAsync();

            return reminders;
        }
    }
}
