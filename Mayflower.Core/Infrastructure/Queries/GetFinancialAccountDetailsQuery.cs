using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Queries
{
    public class GetFinancialAccountDetailsQuery : IQuery<FinancialAccount>, ICacheQuery
    {
        public GetFinancialAccountDetailsQuery() { }

        public int AccountId { get; set; }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = string.Format("GetFinancialAccountDetailsQuery-{0}", ToString()), AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) };

        public override string ToString()
        {
            return string.Format("[AccountId={0}]", AccountId);
        }

    }

    public class GetFinancialAccountDetailsQueryHandler : IQueryHandler<GetFinancialAccountDetailsQuery, FinancialAccount>
    {
        private readonly MayflowerContext _db;

        public GetFinancialAccountDetailsQueryHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<FinancialAccount> HandleAsync(GetFinancialAccountDetailsQuery query)
        {
            var account = await _db.FinancialAccounts.Where(t => t.Id == query.AccountId).Include(t => t.Transactions).Include(t => t.FinancialInstitution).FirstOrDefaultAsync();

            return account ?? new FinancialAccount();
        }
    }
}
