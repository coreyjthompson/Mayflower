using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Queries
{
    public class GetFinancialAccountsQuery : IQuery<IList<FinancialAccount>>, ICacheQuery
    {
        public GetFinancialAccountsQuery() { }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = "GetFinancialAccountsQuery", AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) };
    }

    public class GetFinancialAccountsQueryHandler : IQueryHandler<GetFinancialAccountsQuery, IList<FinancialAccount>>
    {
        private readonly MayflowerContext _db;

        public GetFinancialAccountsQueryHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<IList<FinancialAccount>> HandleAsync(GetFinancialAccountsQuery query)
        {
            return await _db.FinancialAccounts.ToListAsync();
        }
    }
}
