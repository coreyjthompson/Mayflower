using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Data;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Microsoft.EntityFrameworkCore;

namespace Mayflower.Core.Infrastructure.Commands.FinancialAccounts
{
    public class UpdateFinancialAccountCommand : ICommand<bool>
    {
        public UpdateFinancialAccountCommand() { }

        public int Id { get; set; }
        public string Number { get; set; } = string.Empty;
        public FinancialAccountStyle FinancialAccountTheme { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public int FinancialInstitutionId { get; set; }
        public decimal LedgerBalance { get; set; }
        public decimal AvailableBalance { get; set; }
    }

    public class UpdateFinancialAccountCommandHandler : ICommandHandler<UpdateFinancialAccountCommand, bool>
    {
        private readonly MayflowerContext _db;

        public UpdateFinancialAccountCommandHandler(IDbContextFactory<MayflowerContext> dbFactory)
        {
            _db = dbFactory.CreateDbContext();
        }

        public async Task<bool> HandleAsync(UpdateFinancialAccountCommand command)
        {
            var account = new FinancialAccount();

            if (command.Id > 0)
            {
                account = _db.FinancialAccounts?.FirstOrDefault(r => r.Id == command.Id) ?? new FinancialAccount();

                if (account.Id == 0)
                {
                    return false;
                }

                account.Nickname = command.Nickname;
                account.Number = command.Number;
                account.FinancialAccountTheme = command.FinancialAccountTheme;
                account.LedgerBalance = command.LedgerBalance;
                account.AvailableBalance = command.AvailableBalance;
                account.FinancialInstitutionId = command.FinancialInstitutionId;
                account.WhenLastUpdated = DateTimeOffset.Now;
            }

            await _db.SaveChangesAsync();

            if (account.Id != 0)
            {
                return true;
            }

            return false;
        }

    }
}
