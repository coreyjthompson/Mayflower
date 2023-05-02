using Mayflower.Core.DomainModels;
using Microsoft.VisualBasic;
using QFXparser;
using System.Globalization;

namespace Mayflower.Web.Data
{
    public class TransactionService
    {
        private const string BASEPATH = @"D:\Development\GitRepos\Mayflower\TransactionRecordsByAccountId";
        private const string FILENAME = "transactions.qfx";

        public Task<FinancialStatement> GetStatementByAccountId(int accountId)
        {
            var file = BASEPATH + @"\" + accountId + @"\" + FILENAME;

            if (File.Exists(file))
            {
                FileParser parser = new FileParser(file);
                Statement result = parser.BuildStatement();

                return Task.FromResult(MapQfxStatementToFinancialStatement(result));
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

        private Core.DomainModels.Transaction MapQfxTransactionToTransaction(QFXparser.Transaction transaction)
        {
            return new Core.DomainModels.Transaction
            {
                Type = transaction.Type,
                FinancialTransactionId = transaction.TransactionId,
                Amount = transaction.Amount,
                Name = transaction.Name,
                Memo = transaction.Memo,
                PostedOn = transaction.PostedOn,
                RefNumber = transaction.RefNumber,
            };
        }
    }
}