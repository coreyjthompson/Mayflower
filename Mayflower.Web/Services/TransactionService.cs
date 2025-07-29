using Mayflower.Core.DomainModels;
using Microsoft.VisualBasic;
using QFXparser;
using System.Globalization;

namespace Mayflower.Web.Services
{
    public class TransactionService
    {
        private const string BASEPATH = @"D:\Development\Personal\Mayflower\TransactionRecordsByAccountId";
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

        private FinancialStatement MapQfxStatementToFinancialStatement(Statement statement)
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

        private FinancialTransaction MapQfxTransactionToTransaction(Transaction transaction)
        {
            return new FinancialTransaction
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