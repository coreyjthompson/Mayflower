using System.ComponentModel;
using global::Microsoft.AspNetCore.Components;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Extensions;
using Mayflower.Core.Infrastructure.Commands.Reminders;
using Mayflower.Core.Infrastructure.Queries;

namespace Mayflower.Web.Components.Reminders
{
    public partial class SeriesForm
    {
        [Parameter]
        public Reminder? Reminder { get; set; } = null;

        private ReminderFormViewModel _reminderForm = new ReminderFormViewModel();
        private IList<FormControlItem> _reminderStyles = new List<FormControlItem>();
        private IList<FormControlItem> Frequencies = new List<FormControlItem>();
        private IList<FormControlItem> Occurences = new List<FormControlItem>();
        private IList<FormControlItem> DaysOfWeek = new List<FormControlItem>();
        private IList<int> _daysOfMonth = new List<int>();
        private IList<FinancialAccount>? _accounts = null;

        private bool ShowOneTimeFieldSet { get; set; }
        private bool ShowWeeklyFieldSet { get; set; }
        private bool ShowMonthlyFieldSet { get; set; }
        private bool ShowYearlyFieldSet { get; set; }
        private bool ShowTwiceAMonthFieldSet { get; set; }
        private bool ShowEveryQuarterFieldSet { get; set; }
        private bool ShowEveryXDaysFieldSet { get; set; }
        private bool ShowEveryXWeeksFieldSet { get; set; }
        private bool ShowEveryXMonthsFieldSet { get; set; }
        private bool ShowEveryXYearsFieldSet { get; set; }
        private bool ShowMonthlyWeekTogglePanel { get; set; }
        private bool ShowMonthlyMonthTogglePanel { get; set; }
        private bool ShowMonthlyMonthAlert { get; set; }

        private IList<ButtonViewModel> MonthlyToggleButtons { get; set; } = new List<ButtonViewModel>();

        protected override async Task OnInitializedAsync()
        {
            _accounts = await GetAccountsAsync();
            Occurences = GetOccurences();
            DaysOfWeek = GetDaysOfWeek();
            _daysOfMonth = GetDaysOfMonth();
            _reminderStyles = GetReminderStyles();
            Frequencies = GetFrequencyOptions();
            SetMontlhyToggleButtons();
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
            if (reminder == null)
            {
                return;
            }

            _reminderForm = new ReminderFormViewModel
            {
                Id = reminder.Id,
                Amount = reminder.Amount,
                WhenBegins = reminder.WhenBegins,
                WhenExpires = reminder.WhenExpires,
                Type = reminder.ReminderTheme,
                Description = reminder.Description,
                ToAccountId = reminder.TransactionToAccountId?.ToString(),
                FromAccountId = reminder.TransactionFromAccountId?.ToString(),
                RecurrenceInterval = reminder.RecurrenceInterval,
                DayOfTheMonth = reminder.RecurrenceDayOfMonth,
                DayOfTheWeek = reminder.RecurrenceDayOfWeek,
                RecurrenceOrdinal = reminder.RecurrenceOrdinal
            };
        }

        private IList<FormControlItem> GetOccurences()
        {
            var list = new List<FormControlItem>();

            foreach (var item in Enum.GetValues(typeof(RecurrencePosition)))
            {
                var position = (RecurrencePosition)item;
                var description = position.ToDescription();

                if (position == RecurrencePosition.None)
                {
                    description = string.Empty;
                }

                list.Add(new FormControlItem
                {
                    Text = description,
                    Value = ((int)position).ToString(),
                    IsSelected = false
                });
            }

            return list;
        }

        private IList<FormControlItem> GetDaysOfWeek()
        {
            var list = new List<FormControlItem>();

            foreach (var item in Enum.GetValues(typeof(DayOfWeek)))
            {
                var day = (DayOfWeek)item;
                var description = day.ToName();
                var isSelected = (Reminder?.RecurrenceDayOfWeek == day);

                if (!isSelected)
                {
                    isSelected = (Reminder?.WhenBegins.DayOfWeek == day);
                }

                list.Add(new FormControlItem
                {
                    Text = description,
                    Value = ((int)day).ToString(),
                    IsSelected = isSelected
                });
            }

            return list;
        }

        private IList<FormControlItem> GetReminderStyles()
        {
            var list = new List<FormControlItem>();

            foreach (var item in Enum.GetValues(typeof(ReminderStyle)))
            {
                var style = (ReminderStyle)item;

                if (style != ReminderStyle.Error)
                {
                    var description = style.ToDescription();

                    list.Add(new FormControlItem
                    {
                        Text = description,
                        Value = ((int)style).ToString(),
                        IsSelected = (Reminder?.ReminderTheme == style)
                    });
                }

            }

            return list;
        }

        private void SetMontlhyToggleButtons()
        {
            ShowMonthlyMonthTogglePanel = Reminder?.RecurrenceDayOfMonth != null;
            ShowMonthlyWeekTogglePanel = Reminder?.RecurrenceDayOfWeek != null;

            // If neither are set
            if (!ShowMonthlyMonthTogglePanel && !ShowMonthlyWeekTogglePanel)
            {
                // Set the default
                ShowMonthlyMonthTogglePanel = true;
            }

            MonthlyToggleButtons = new List<ButtonViewModel>
            {
                new ButtonViewModel("Day of the Month", "month", "nav-link", ShowMonthlyMonthTogglePanel),
                new ButtonViewModel("Day of the Week", "week", "nav-link", ShowMonthlyWeekTogglePanel),
            };

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

        private IList<FormControlItem> GetFrequencyOptions()
        {
            var list = new List<FormControlItem>();
            var current = GetCurrentFrequency(Reminder?.RecurrenceTheme, Reminder?.RecurrenceInterval);

            foreach (var item in Enum.GetValues(typeof(ReminderFrequency)))
            {
                var type = (ReminderFrequency)item;
                var description = type.ToDescription();

                list.Add(new FormControlItem
                {
                    Text = description,
                    Value = ((int)type).ToString(),
                    IsSelected = (current == type)
                });
            }

            ShowFormFieldSetByFrequency(current);

            return list;
        }

        private ReminderFrequency GetCurrentFrequency(RecurrenceStyle? recurrenceStyle, int? interval = 0)
        {
            switch (recurrenceStyle)
            {
                case RecurrenceStyle.NoRecurrence:
                    return ReminderFrequency.None;

                case RecurrenceStyle.Year:
                    if (interval > 1)
                    {
                        return ReminderFrequency.EveryXYears;
                    }

                    return ReminderFrequency.EveryYear;

                case RecurrenceStyle.Month:
                    if (interval == 3)
                    {
                        return ReminderFrequency.EveryQuarter;
                    }
                    else if (interval > 1)
                    {
                        return ReminderFrequency.EveryXMonths;
                    }

                    return ReminderFrequency.EveryMonth;

                case RecurrenceStyle.Week:
                    if (interval > 1)
                    {
                        return ReminderFrequency.EveryXWeeks;
                    }

                    return ReminderFrequency.EveryWeek;

                case RecurrenceStyle.Day:
                    return ReminderFrequency.EveryXDays;
            }

            return ReminderFrequency.None;
        }

        private string GetOrdinalNumberFromNumber(int number)
        {
            if (number <= 0)
            {
                return number.ToString();
            }

            switch (number % 100)
            {
                case 11:
                case 12:
                case 13:
                    return number + "th";
            }

            switch (number % 10)
            {
                case 1:
                    return number + "st";
                case 2:
                    return number + "nd";
                case 3:
                    return number + "rd";
                default:
                    return number + "th";
            }
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

        private void HandleFrequencyChange(ChangeEventArgs e)
        {
            if (e == null || e.Value == null)
            {
                return;
            }

            ReminderFrequency frequency = ReminderFrequency.None;

            if (int.TryParse(e.Value.ToString(), out int value))
            {
                frequency = (ReminderFrequency)value;
            }

            ShowFormFieldSetByFrequency(frequency);
        }

        private void HandleMonthlyToggleButtonClick(ButtonViewModel selectedButton)
        {
            foreach (var button in MonthlyToggleButtons)
            {
                if (button.Value == selectedButton.Value)
                {
                    button.IsActive = true;

                    if (button.Value == "week")
                    {
                        ShowMonthlyWeekTogglePanel = true;
                        ShowMonthlyMonthTogglePanel = false;
                    }
                    else if (button.Value == "month")
                    {
                        ShowMonthlyWeekTogglePanel = false;
                        ShowMonthlyMonthTogglePanel = true;
                    }
                }
                else
                {
                    button.IsActive = false;
                }

            }
        }

        private void ShowFormFieldSet(bool? showOneTimeFieldSet = false, bool? showWeeklyFieldSet = false, bool? showMonthlyFieldSet = false, bool? showYearlyFieldSet = false, bool? showTwiceFieldSet = false,
            bool? showQuarterlyFieldSet = false, bool? showXDaysFieldSet = false, bool? showXWeeksFieldSet = false, bool? showXMonthsFieldSet = false, bool? showXYearsFieldSet = false)
        {
            ShowOneTimeFieldSet = showOneTimeFieldSet ?? false;
            ShowWeeklyFieldSet = showWeeklyFieldSet ?? false;
            ShowMonthlyFieldSet = showMonthlyFieldSet ?? false;
            ShowYearlyFieldSet = showYearlyFieldSet ?? false;
            ShowTwiceAMonthFieldSet = showTwiceFieldSet ?? false;
            ShowEveryQuarterFieldSet = showQuarterlyFieldSet ?? false;
            ShowEveryXDaysFieldSet = showXDaysFieldSet ?? false;
            ShowEveryXWeeksFieldSet = showXWeeksFieldSet ?? false;
            ShowEveryXMonthsFieldSet = showXMonthsFieldSet ?? false;
            ShowEveryXYearsFieldSet = showXYearsFieldSet ?? false;
        }

        private void ShowFormFieldSetByFrequency(ReminderFrequency frequency)
        {
            switch (frequency)
            {
                case ReminderFrequency.None:
                default:
                    ShowFormFieldSet(true);
                    break;
                case ReminderFrequency.EveryWeek:
                    ShowFormFieldSet(false, true);
                    break;
                case ReminderFrequency.EveryMonth:
                    ShowFormFieldSet(false, false, true);
                    break;
                case ReminderFrequency.EveryYear:
                    ShowFormFieldSet(false, false, false, true);
                    break;
                case ReminderFrequency.TwiceAMonth:
                    ShowFormFieldSet(false, false, false, false, true);
                    break;
                case ReminderFrequency.EveryQuarter:
                    ShowFormFieldSet(false, false, false, false, false, true);
                    break;
                case ReminderFrequency.EveryXDays:
                    ShowFormFieldSet(false, false, false, false, false, false, true);
                    return;
                case ReminderFrequency.EveryXWeeks:
                    ShowFormFieldSet(false, false, false, false, false, false, false, true);
                    return;
                case ReminderFrequency.EveryXMonths:
                    ShowFormFieldSet(false, false, false, false, false, false, false, false, true);
                    return;
                case ReminderFrequency.EveryXYears:
                    ShowFormFieldSet(false, false, false, false, false, false, false, false, false, true);
                    return;
            }

        }

        public async Task<bool> Save()
        {
            // TODO: add validation of some sort
            var command = new InsertUpdateReminderCommand
            {
                Id = _reminderForm.Id,
                WhenBegins = _reminderForm.WhenBegins,
                WhenExpires = _reminderForm.WhenExpires,
                Amount = _reminderForm.Amount,
                ReminderTheme = _reminderForm.Type,
                Description = _reminderForm.Description,
                TransactionToAccountId = _reminderForm.ToAccountId,
                TransactionFromAccountId = _reminderForm.FromAccountId,
                RecurrenceDayOfMonth = _reminderForm.DayOfTheMonth,
                RecurrenceDayOfWeek = _reminderForm.DayOfTheWeek,
                RecurrenceInterval = _reminderForm.RecurrenceInterval,
                RecurrenceOrdinal = _reminderForm.RecurrenceOrdinal,
                RecurrenceTheme = MapFrequencyToRecurrenceType(_reminderForm.Frequency)
            };

            return await _commands.Execute(command);
        }

        private RecurrenceStyle MapFrequencyToRecurrenceType(ReminderFrequency frequency)
        {
            switch (frequency)
            {
                case ReminderFrequency.None:
                default:
                    return RecurrenceStyle.NoRecurrence;

                case ReminderFrequency.EveryYear:
                case ReminderFrequency.EveryXYears:
                    return RecurrenceStyle.Year;

                case ReminderFrequency.EveryMonth:
                case ReminderFrequency.EveryQuarter:
                case ReminderFrequency.EveryXMonths:
                    return RecurrenceStyle.Month;

                case ReminderFrequency.EveryWeek:
                case ReminderFrequency.EveryXWeeks:
                    return RecurrenceStyle.Week;

                case ReminderFrequency.EveryXDays:
                    return RecurrenceStyle.Day;
            }
        }


        public class ReminderFormViewModel
        {
            public int Id { get; set; }
            public DateOnly WhenBegins { get; set; }
            public DateOnly? WhenExpires { get; set; }
            public decimal Amount { get; set; }
            public ReminderStyle Type { get; set; }
            public string? Description { get; set; }
            public string? ToAccountId { get; set; }
            public string? FromAccountId { get; set; }
            public ReminderFrequency Frequency { get; set; }
            public int RecurrenceInterval { get; set; }
            public int? DayOfTheMonth { get; set; }
            public DayOfWeek? DayOfTheWeek { get; set; }
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

        private class ButtonViewModel
        {
            private string _incomingCssClass = string.Empty;
            public ButtonViewModel(string? text, string? value, string cssClass, bool? isActive = false)
            {
                Text = text ?? string.Empty;
                Value = value ?? string.Empty;
                IsActive = isActive ?? false;
                _incomingCssClass = cssClass ?? string.Empty;
            }

            public string Text { get; set; } = string.Empty;
            public string Value { get; set; } = string.Empty;
            public string CssClass => GetButtonStateCssClass();
            public bool IsActive { get; set; }

            string GetButtonStateCssClass()
            {
                var cssClass = _incomingCssClass;

                if (IsActive)
                {
                    cssClass += " active";
                }

                return cssClass;
            }

        }

        public class FormControlItem
        {
            public bool IsSelected { get; set; }
            public bool IsDisabled { get; set; }
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