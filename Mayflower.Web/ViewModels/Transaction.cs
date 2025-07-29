namespace Mayflower.Web.ViewModels
{
    public class Transaction
    {
        public int Id { get; set; }

        public DateTime PostedOn { get; set; }

        public string PostedOnForDisplay
        {
            get
            {
                return PostedOn.ToString("MM/dd/yyyy");
            }
        }

        public Decimal Amount { get; set; }

        public string AmountForDisplay
        {
            get
            {
                return Amount.ToString("#,##0.00");
            }
        }

        public string Name { get; set; } = default!;

    }
}
