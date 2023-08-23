using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Commands.FinancialTransactions
{
    public class InsertTransactionsCommand : ICommand<int>
    {
        public InsertTransactionsCommand() { }

        public int AccountId { get; set; }

        public IList<DomainModels.FinancialTransaction> Transactions { get; set; } = new List<DomainModels.FinancialTransaction>();

        public decimal LedgerBalance { get; set; }

        public decimal AvailableBalance { get; set; }

        public override string ToString()
        {
            return string.Format("[AccountId={0}, TransactionCount={1}, LedgerBalance={2}, AvailableBalance={3}]", AccountId, Transactions.Count, LedgerBalance, AvailableBalance);
        }
    }

    public class InsertTransactionsCommandHandler : ICommandHandler<InsertTransactionsCommand, int>
    {
        private readonly MayflowerContext _db;

        public InsertTransactionsCommandHandler(MayflowerContext db)
        {
            _db = db;
        }

        public async Task<int> HandleAsync(InsertTransactionsCommand command)
        {
            if(command.AccountId == 0)
            {
                return 0;
            }

            var insertedCount = 0;

            foreach (var transaction in command.Transactions)
            {
                var transactionEntity = _db.Transactions.Where(t => t.ExternalTransactionId == transaction.ExternalTransactionId && t.FinancialAccountId == command.AccountId).FirstOrDefault();
                if (transactionEntity == null)
                {
                    // The transaction wasnt found so add it
                    transaction.FinancialAccountId = command.AccountId;
                    transaction.FinancialAccount = null;

                    await _db.Transactions.AddAsync(transaction);

                    // Increment the count so we can return it later
                    insertedCount++;
                }
            }

            var accountEntity = _db.FinancialAccounts.Where(a => a.Id == command.AccountId).FirstOrDefault();

            if (accountEntity != null)
            {
                accountEntity.AvailableBalance = command.AvailableBalance;
                accountEntity.LedgerBalance = command.LedgerBalance;
                accountEntity.WhenLastUpdated = DateTimeOffset.Now;
            }

            await _db.SaveChangesAsync();

            return insertedCount;
        }

    }
}
