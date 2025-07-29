using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using QFXparser;

namespace Mayflower.Core.Infrastructure.Queries
{
    public class GetLatestStatementByAccountIdQuery : IQuery<FinancialStatement>
    {
        public GetLatestStatementByAccountIdQuery() { }

        public int AccountId { get; set; }

        public CacheQueryOptions CacheQueryOptions =>
            new CacheQueryOptions { CacheKey = string.Format("GetLatestStatementByAccountIdQuery-{0}", ToString()), AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(30) };

        public override string ToString()
        {
            return string.Format("[AccountId={0}]", AccountId);
        }

    }

    public class GetLatestStatementByAccountIdQueryHandler : IQueryHandler<GetLatestStatementByAccountIdQuery, FinancialStatement>
    {
        private const string BASEPATH = @"D:\Development\Personal\Mayflower\TransactionRecordsByAccountId";
        //TODO: Use string applicationRoot = AppDomain.CurrentDomain.BaseDirectory;
        private const string FILENAME = "transactions.qfx";

        public GetLatestStatementByAccountIdQueryHandler()
        {
        }

        public async Task<FinancialStatement> HandleAsync(GetLatestStatementByAccountIdQuery query)
        {
            var file = BASEPATH + @"\" + query.AccountId + @"\" + FILENAME;

            if (File.Exists(file))
            {
                FileParser parser = new FileParser(file);
                Statement result = parser.BuildStatement();

                return await Task.FromResult(MapQfxStatementToFinancialStatement(result));
            }

            return default!;
        }

        private FinancialStatement MapQfxStatementToFinancialStatement(QFXparser.Statement statement)
        {
            return new FinancialStatement
            {
                AccountNumber = statement.AccountNum,
                LedgerBalance = new Balance
                {
                    Amount = statement.LedgerBalance.Amount,
                    AsOf = statement.LedgerBalance.AsOf
                },
                AvailableBalance = new Balance
                {
                    Amount = statement.AvailableBalance.Amount,
                    AsOf = statement.AvailableBalance.AsOf
                },
                Transactions = statement.Transactions.Select(t => MapQfxTransactionToTransaction(t)).ToList()
            };
        }

        private DomainModels.FinancialTransaction MapQfxTransactionToTransaction(QFXparser.Transaction transaction)
        {
            return new DomainModels.FinancialTransaction
            {
                Type = transaction.Type,
                ExternalTransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                Name = transaction.Name,
                Memo = transaction.Memo,
                WhenPosted = DateOnly.FromDateTime(transaction.PostedOn),
                RefNumber = transaction.RefNumber,
            };
        }

    }
}
