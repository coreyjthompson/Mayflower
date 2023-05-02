using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Reflection.PortableExecutable;

namespace Mayflower.Core.Infrastructure.Queries.Reminders
{
    public class GetAllRemindersWithinDateRange : IQuery<List<Reminder>>, ICacheQuery
    {
        public DateOnly StartDate { get; set; }

        public DateOnly EndDate { get; set; }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = string.Format("GetAllRemindersWithinDateRange-{0}", ToString()), SlidingExpiration = TimeSpan.FromMinutes(2) };

        public override string ToString()
        {
            return string.Format("[StartDate={0}, EndDate={1}]", StartDate, EndDate);
        }
    }

    public class GetAllRemindersWithinDateRangeQueryHandler : IQueryHandler<GetAllRemindersWithinDateRange, List<Reminder>>
    {
        private readonly MayflowerContext _db;

        public GetAllRemindersWithinDateRangeQueryHandler(MayflowerContext db)
        {
            _db = db;
        }

        public async Task<List<Reminder>> HandleAsync(GetAllRemindersWithinDateRange query)
        {
            return await Task.FromResult(new List<Reminder>());
        }
    }
}
