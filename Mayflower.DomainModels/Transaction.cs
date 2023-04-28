namespace Mayflower.DomainModels
{
    public class Transaction
    {
        public DateOnly Date { get; set; }

        public TimeOnly Time { get; set; }

        public decimal Amount { get; set; }

        public string Type { get; set; } = "Withdraw";

        public string Description { get; set; } = "NA";
    }
}