using System.Collections.Generic;

namespace Mayflower.Core.DomainModels
{
    public class FinancialStatement
    {
        public string AccountNumber { get; set; } = default!;
        public IList<FinancialTransaction> Transactions { get; set; } = new List<FinancialTransaction>();
        public Balance LedgerBalance { get; set; } = new Balance();
        public Balance AvailableBalance { get; set; } = new Balance();

    }
}
