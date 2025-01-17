using global::Microsoft.AspNetCore.Components;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Extensions;
using Mayflower.Core.Infrastructure.Queries;
using System.ComponentModel;

namespace Mayflower.Web.Components
{
    public partial class ReminderSeriesForm
    {
        [Parameter]
        public Reminder? Reminder { get; set; } = null;

        private ReminderForm _reminderForm = new ReminderForm();
        private IList<(ReminderStyle, string, string)> _reminderStyles = new List<(ReminderStyle, string, string)>();
        private IList<FormControlItem> _frequencies = new List<FormControlItem>();
        private IList<(RecurrencePosition, string, string)> _positions = new List<(RecurrencePosition, string, string)>();
        private IList<(DayOfWeek, string, string)> _daysOfWeek = new List<(DayOfWeek, string, string)>();
        private IList<int> _daysOfMonth = new List<int>();
        private IList<FinancialAccount>? _accounts = null;

        protected bool ShowOneTimeFieldSet { get; set; }
        protected bool ShowWeeklyFieldSet { get; set; }
        protected bool ShowMonthlyFieldSet { get; set; }

        protected override async Task OnInitializedAsync()
        {
            _accounts = await GetAccountsAsync();
            _positions = GetPositions();
            _daysOfWeek = GetDaysOfWeek();
            _daysOfMonth = GetDaysOfMonth();
            _reminderStyles = GetReminderStyles();
            _frequencies = GetFrequencyChoices();

            ShowOneTimeFieldSet = true;
        }

        protected override void OnParametersSet()
        {
            SetReminderFormData(Reminder);
        }

        private async Task<IList<FinancialAccount>> GetAccountsAsync()
        {
            var query = new GetFinancialAccountsQuery();
            return await _queries.Execute(query);
        }

        private void SetReminderFormData(Reminder? reminder)
        {
            if (reminder != null)
            {
                _reminderForm = new ReminderForm
                {
                    Id = reminder.Id,
                    Amount = reminder.Amount,
                    WhenBegins = reminder.WhenBegins,
                    WhenExpires = reminder.WhenExpires,
                    ReminderTheme = reminder.ReminderTheme,
                    Description = reminder.Description,
                    TransactionToAccountId = reminder.TransactionToAccountId,
                    TransactionFromAccountId = reminder.TransactionFromAccountId,
                    RecurrenceTheme = reminder.RecurrenceTheme,
                    RecurrenceInterval = reminder.RecurrenceInterval,
                    RecurrenceDayOfMonth = reminder.RecurrenceDayOfMonth,
                    RecurrenceDayOfWeek = reminder.RecurrenceDayOfWeek,
                    RecurrenceOrdinal = reminder.RecurrenceOrdinal
                };
            } 
            else
            {
                // If its a new reminder, set up the form
            }
        }

        private IList<(RecurrencePosition, string, string)> GetPositions()
        {
            var list = new List<(RecurrencePosition, string, string)>();

            foreach (var item in Enum.GetValues(typeof(RecurrencePosition)))
            {
                var position = (RecurrencePosition)item;

                if (position != RecurrencePosition.None)
                {
                    var description = position.ToDescription();

                    list.Add((position, ((int)position).ToString(), description));
                }
            }

            return list;
        }

        private IList<(DayOfWeek, string, string)> GetDaysOfWeek()
        {
            var list = new List<(DayOfWeek, string, string)>();

            foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
            {
                var day = (DayOfWeek)item;
                var description = day.ToName();

                list.Add((day, ((int)day).ToString(), description));
            }

            return list;
        }

        private IList<(ReminderStyle, string, string)> GetReminderStyles()
        {
            var list = new List<(ReminderStyle, string, string)>();

            foreach (var item in Enum.GetValues(typeof(ReminderStyle)))
            {
                var style = (ReminderStyle)item;

                if (style != ReminderStyle.Error)
                {
                    var description = style.ToDescription();

                    list.Add((style, ((int)style).ToString(), description));
                }

            }

            return list;
        }

        private IList<(RecurrenceStyle, string, string)> GetRecurrenceStyles()
        {
            var list = new List<(RecurrenceStyle, string, string)>();

            foreach (var item in Enum.GetValues(typeof(RecurrenceStyle)))
            {
                var style = (RecurrenceStyle)item;
                var description = style.ToDescription();

                list.Add((style, ((int)style).ToString(), description));
            }

            return list;
        }

        private IList<FormControlItem> GetFrequencyChoices()
        {
            var list = new List<FormControlItem>();

            foreach (var item in Enum.GetValues(typeof(ReminderFrequency)))
            {
                var type = (ReminderFrequency)item;
                var description = type.ToDescription();

                list.Add(new FormControlItem
                {
                    Text = description,
                    Value = type.ToString(),
                    OriginalEnum = type
                });
            }

            return list;
        }

        private IList<int> GetDaysOfMonth()
        {
            var list = new List<int>();

            for (int i = 1; i <= 31; i++)
            {
                list.Add(i);
            }

            return list;
        }

        private void HandlePositionChange(ChangeEventArgs e)
        {
        }

        private void HandleDayOfWeekChange(ChangeEventArgs e)
        {
        }

        private void HandleDayOfMonthChange(ChangeEventArgs e)
        {
        }

        private void HandleThemeChange(ChangeEventArgs e)
        {
        }

        private void HandleTransactionToAccountChange(ChangeEventArgs e)
        {
        }

        private void HandleTransactionFromAccountChange(ChangeEventArgs e)
        {
        }

        public class ReminderForm
        {
            public int Id { get; set; }
            public DateOnly WhenBegins { get; set; }
            public DateOnly? WhenExpires { get; set; }
            public decimal Amount { get; set; }
            public ReminderStyle ReminderTheme { get; set; }
            public string? Description { get; set; }
            public int? TransactionToAccountId { get; set; }
            public int? TransactionFromAccountId { get; set; }
            public RecurrenceStyle RecurrenceTheme { get; set; }
            public int RecurrenceInterval { get; set; }
            public int? RecurrenceDayOfMonth { get; set; }
            public DayOfWeek? RecurrenceDayOfWeek { get; set; }
            public RecurrencePosition RecurrenceOrdinal { get; set; }

            #region Labels
            public string WhenBeginsLabelText { get; set; } = "Start Date";
            public string WhenExpiresLabelText { get; set; } = "End Date";
            public string AmountLabelText { get; set; } = "Recurring Amount";
            public string ReminderThemeLabelText { get; set; } = "Type";
            public string DescriptionLabelText { get; set; } = "Description";
            public string TransactionToLabelText { get; set; } = "Select an account";
            public string TransactionFromLabelText { get; set; } = "Select an account";
            public string RecurrenceThemeLabelText { get; set; } = "Frequency";
            public string RecurrenceIntervalLabelText { get; set; } = "Interval";
            public string RecurrenceDayOfMonthLabelText { get; set; } = "Day of the month";
            public string RecurrenceDayOfWeekLabelText { get; set; } = "Day of the week";
            public string RecurrenceOrdinalLabelText { get; set; } = "Ordinal";
            #endregion

            #region Placeholders
            public string WhenBeginsPlaceholderText { get; set; } = default!;
            public string WhenExpiresPlaceholderText { get; set; } = default!;
            public string AmountPlaceholderText { get; set; } = "Amount";
            public string ReminderThemePlaceholderText { get; set; } = "Type";
            public string DescriptionPlaceholderText { get; set; } = "Description";
            public string TransactionToPlaceholderText { get; set; } = "Select an account";
            public string TransactionFromPlaceholderText { get; set; } = "Select an account";
            public string RecurrenceThemePlaceholderText { get; set; } = default!;
            public string RecurrenceIntervalPlaceholderText { get; set; } = "Interval";
            public string RecurrenceDayOfMonthPlaceholderText { get; set; } = "Select a day";
            public string RecurrenceDayOfWeekPlaceholderText { get; set; } = "Select a day";
            public string RecurrenceOrdinalPlaceholderText { get; set; } = "Select an ordinal";
            #endregion
        }

        public class FormControlItem
        {
            public bool IsSelected { get; set; }
            public bool IsDisabled { get; set; }
            public dynamic OriginalEnum { get; set; } = default!;
            public string Text { get; set; } = default!;
            public string Value { get; set; } = default!;
        }

        public enum ReminderFrequency
        {
            [Description("One-time payment")]
            None,
            [Description("Every week")]
            EveryWeek,
            [Description("Every month")]
            EveryMonth,
            [Description("Every year")]
            EveryYear,
            [Description("Twice a month")]
            TwiceAMonth,
            [Description("Every quarter")]
            EveryQuarter,
            [Description("Every X days")]
            EveryXDays,
            [Description("Every X weeks")]
            EveryXWeeks,
            [Description("Every X months")]
            EveryXMonths,
            [Description("Every X years")]
            EveryXYears
        }
    }
}