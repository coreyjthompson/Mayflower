using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Queries
{
    public class GetTransactionsQuery : IQuery<IList<FinancialTransaction>>, ICacheQuery
    {
        public GetTransactionsQuery() { }

        public int AccountId { get; set; }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = string.Format("GetTransactionsQuery-{0}", ToString()), AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) };

        public override string ToString()
        {
            return string.Format("[AccountId={0}]", AccountId);
        }

    }

    public class GetTransactionsQueryHandler : IQueryHandler<GetTransactionsQuery, IList<FinancialTransaction>>
    {
        private readonly MayflowerContext _db;

        public GetTransactionsQueryHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<IList<FinancialTransaction>> HandleAsync(GetTransactionsQuery query)
        {

            return await _db.Transactions.Where(t => t.Id == query.AccountId).ToListAsync();
        }
    }
}
