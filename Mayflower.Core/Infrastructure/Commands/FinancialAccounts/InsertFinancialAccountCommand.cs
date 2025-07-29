using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Commands.Reminders
{
    public class InsertFinancialAccountCommand : ICommand<bool>
    {
        public InsertFinancialAccountCommand() { }

        public string Number { get; set; } = string.Empty;
        public FinancialAccountStyle FinancialAccountTheme { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public int FinancialInstitutionId { get; set; }
        public decimal LedgerBalance { get; set; }
        public decimal AvailableBalance { get; set; }
    }

    public class InsertFinancialAccountCommandHandler : ICommandHandler<InsertFinancialAccountCommand, bool>
    {
        private readonly MayflowerContext _db;

        public InsertFinancialAccountCommandHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<bool> HandleAsync(InsertFinancialAccountCommand command)
        {
            var account = new FinancialAccount
            {
                Nickname = command.Nickname,
                Number = command.Number,
                FinancialInstitutionId = command.FinancialInstitutionId,
                FinancialAccountTheme = command.FinancialAccountTheme,
                LedgerBalance = command.LedgerBalance,
                AvailableBalance = command.AvailableBalance,
                WhenLastUpdated = DateTimeOffset.Now
            };

            await _db.FinancialAccounts.AddAsync(account);
            await _db.SaveChangesAsync();

            if (account.Id != 0)
            {
                return true;
            }

            return false;
        }

    }
}
