using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mayflower.Core.DomainModels
{
    public class Transaction
    {
        public int Id { get; set; }

        public string Type { get; set; } = default!;

        public string FinancialTransactionId { get; set; } = default!;

        public DateTime PostedOn { get; set; }

        public Decimal Amount { get; set; }

        public string RefNumber { get; set; } = default!;

        public string Name { get; set; } = default!;

        public string Memo { get; set; } = default!;

    }
}