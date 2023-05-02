using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.PortableExecutable;

namespace Mayflower.Core.Infrastructure.Queries.Reminders
{
    public class GetAllRemindersByFinancialAccountIdQuery : IQuery<List<Reminder>>
    {
        public int Id { get; set; }

        public GetAllRemindersByFinancialAccountIdQuery(int id)
        {
            Id = id;
        }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = string.Format("GetAllRemindersByFinancialAccountIdQuery-{0}", ToString()), SlidingExpiration = TimeSpan.FromMinutes(2) };

        public override string ToString()
        {
            return string.Format("[Id={0}]", Id);
        }
    }

    public class GetAllRemindersByFinancialAccountIdQueryHandler : IQueryHandler<GetAllRemindersByFinancialAccountIdQuery, List<Reminder>>
    {
        private readonly MayflowerContext _db;

        public GetAllRemindersByFinancialAccountIdQueryHandler(MayflowerContext db)
        {
            _db = db;
        }

        public async Task<List<Reminder>> HandleAsync(GetAllRemindersByFinancialAccountIdQuery query)
        {
            return await _db.Reminders.Where(r => r.TransactionFromAccountId == query.Id || 
                r.TransactionToAccountId == query.Id).ToListAsync();
        }
    }
}
