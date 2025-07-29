using global::Microsoft.AspNetCore.Components;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Extensions;
using Microsoft.AspNetCore.Html;

namespace Mayflower.Web.Components
{
    public partial class ReminderList
    {
        private const string CURRENCY_FORMAT = "{0:C2}";

        private IList<TableRow>? _rows = null;

        [Parameter]
        public IList<TableRow>? Rows { get; set; }

        protected override void OnInitialized()
        {
            if (Rows != null)
            {
                _rows = Rows;
            }
        }

        protected override void OnParametersSet()
        {
            if (Rows != null)
            {
                _rows = Rows;
            }
        }

        public class TableRow
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public int SeriesId { get; set; }
            public int? OccurrenceId { get; set; }
            public string? RecurrenceDescription { get; set; }
            public HtmlString? NextRecurrenceDetails { get; set; }
            public string Cost  => string.Format(CURRENCY_FORMAT, TransactionAmount);
            public string Description { get; set; } = "Unknown reminder";
            public string? Theme { get; set; }
            public string FromAccountName { get; set; } = "Unknown account";
            public string? TransactionCategory { get; set; }
            public string? FromAccountTheme { get; set; }
            public DateOnly WhenScheduledToNextOccur { get; set; }
            public DateOnly WhenScheduledToFirstOccur { get; set; }
            public Decimal TransactionAmount { get; set; } = 0;

        }

    }
}