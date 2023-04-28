using CsvHelper.Configuration.Attributes;

namespace Mayflower.Web.Dto
{
    public class Transaction
    {
        [Name("Date")]
        public DateOnly Date { get; set; }

        [Name(" Time")]
        public TimeOnly Time { get; set; }

        [Name(" Amount")]
        public decimal Amount { get; set; }

        [Name(" Type")]
        public string Type { get; set; } = "Withdraw";

        [Name(" Description")]
        public string? Description { get; set; }
    }
}