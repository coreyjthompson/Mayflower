using BlazorBootstrap;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Extensions;
using Mayflower.Core.Infrastructure.Queries.Reminders;
using Mayflower.Web.Components.Reminders;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Html;

namespace Mayflower.Web.Pages
{
    public partial class Reminders
    {

        private const string PAGE_TITLE = "Bills & Payments";
        private const string CURRENCY_FORMAT = "{0:C2}";

        private string? _rangeInDays = "30";
        private IList<Reminder> _reminders = new List<Reminder>();
        private IList<ReminderItem>? _recurringReminders = null;
        private IList<ReminderItem>? _upcomingReminders = null;
        private (DateOnly, DateOnly) _dateRange = (DateOnly.FromDateTime(DateTime.Now), DateOnly.FromDateTime(DateTime.Now).AddDays(30));

        private SeriesForm SeriesModalForm { get; set; } = new SeriesForm();
        private Modal SeriesModal { get; set; } = new Modal();
        private Reminder? CurrentSeries { get; set; } = new Reminder();
        private IList<ReminderItem>? FilteredRecurringReminders { get; set; } = default;
        private ReminderSummary Summary { get; set; } = new ReminderSummary();
        private IList<ButtonStateContainer>? RecurringReminderFilterButtons { get; set; } = default;

        protected override async Task OnInitializedAsync()
        {
            UpdateGlobalPageProperties();

            _reminders = await GetRemindersAsync();

            SetReminderItems();
        }

        private async Task<IList<Reminder>> GetRemindersAsync()
        {
            var query = new GetRemindersWithFullDetailsQuery();

            return await _queries.Execute(query);
        }

        private IList<ReminderItem> GetRecurringReminders()
        {
            if (_reminders == null)
            {
                return new List<ReminderItem>();
            }

            return _reminders
                .Where(r => r.RecurrenceTheme != RecurrenceStyle.NoRecurrence)
                .Select(MapReminderToRecurringReminderRow)
                .ToList();
        }

        private IList<ReminderItem> FilterRecurringReminders(string filterDescription)
        {
            if (_recurringReminders == null)
            {
                _recurringReminders = GetRecurringReminders();
            }

            if (_recurringReminders == null)
            {
                return new List<ReminderItem>();
            }

            if (filterDescription == "all")
            {
                return _recurringReminders
                    .OrderBy(r => r.Description)
                    .ToList();
            }

            return _recurringReminders
                .Where(r => r.Theme.ToLower() == filterDescription)
                .OrderBy(r => r.Description)
                .ToList();
        }

        private IList<ReminderItem> GetUpcomingReminders()
        {
            if (_reminders == null)
            {
                return new List<ReminderItem>();
            }

            // Sort them by date before any work is done on them
            var sortedReminders = _reminders.OrderBy(r => r.WhenBegins).ToList();

            var whenSearchBegins = DateOnly.FromDateTime(DateTime.Now);
            var whenSearchEnds = whenSearchBegins.AddDays(30);
            var occurrences = new List<(DateOnly, Reminder)>();

            foreach (var reminder in sortedReminders)
            {
                // Set the date variables that will be used throughout the code
                DateOnly whenFirstOccurs = reminder.WhenBegins;
                DateOnly? whenExpires = reminder.WhenExpires;

                // If it falls within today and activeRangeInDays from now, we want to make at least one row for it
                if ((whenFirstOccurs <= whenSearchBegins || (whenFirstOccurs > whenSearchBegins && whenFirstOccurs <= whenSearchEnds)) && (whenExpires == null || whenExpires >= whenSearchEnds))
                {
                    // Only add those that start today or before and end after today
                    // Get the recurrences
                    if (reminder.RecurrenceTheme == RecurrenceStyle.NoRecurrence)
                    {
                        // No recurrence - single occurence
                        occurrences.Add((reminder.WhenBegins, reminder));
                    }
                    else
                    {
                        if (reminder.RecurrenceTheme == RecurrenceStyle.Day)
                        {
                            // Daily recurrences
                            // Start the loop at whenOccurs and run until our running date is equal to or greater than the end date.
                            // The running date will be our occurrence's display date
                            for (DateOnly runningDate = whenFirstOccurs; runningDate <= whenSearchEnds; runningDate = runningDate.AddDays(reminder.RecurrenceInterval))
                            {
                                occurrences.Add((runningDate, reminder));
                            }
                        }
                        else if (reminder.RecurrenceTheme == RecurrenceStyle.Week)
                        {
                            // Weekly recurrences
                            //RecurrenceInterval X 7 days in a week
                            var daysTillNextOccurrence = reminder.RecurrenceInterval * 7;
                            // Start the loop at whenOccurs and run until our running date is equal to or greater than the end date.
                            // The running date will be our occurrence's display date
                            for (DateOnly runningDate = whenFirstOccurs; runningDate <= whenSearchEnds; runningDate = runningDate.AddDays(daysTillNextOccurrence))
                            {
                                // If this new occurence is between start and end date, add it
                                if (runningDate >= whenSearchBegins)
                                {
                                    occurrences.Add((runningDate, reminder));
                                }
                            }
                        }
                        else if (reminder.RecurrenceTheme == RecurrenceStyle.Month)
                        {
                            // Monthly recurrences
                            // Start the loop at whenOccurs and run until our running date is equal to or greater than the end date.
                            // The running date will be our occurrence's display date
                            for (DateOnly runningDate = whenFirstOccurs; runningDate <= whenSearchEnds; runningDate = runningDate.AddMonths(reminder.RecurrenceInterval))
                            {
                                // If this new occurence is between start and end date, add it
                                if (runningDate >= whenSearchBegins)
                                {
                                    occurrences.Add((runningDate, reminder));
                                }
                            }
                        }
                    }
                }
            }

            // Sort them by date so we can properly calculate them in order
            occurrences = occurrences.OrderBy(r => r.Item1).ToList();
            var filteredOccurences = new List<ReminderItem>();

            // Loop each row, filter them and setup menus by occurrence and sum it all up
            foreach (var item in occurrences)
            {
                var newOccurrence = item.Item2.Occurrences?.FirstOrDefault(o => o.WhenOriginallyScheduledToOccur == item.Item1);
                var row = new ReminderItem();

                if (newOccurrence != null)
                {
                    if (newOccurrence.ReasonForOccurrence == ReminderOccurrenceCause.Edit)
                    {
                        //new HtmlString(string.Format("Next: {0} on {1}", row.Cost, whenNextRecurs.ToString("MMM d, yyyy")));

                        // Grab the occurrence's data
                        row = new ReminderItem
                        {
                            // This should never be null if we have a cause of edit
                            WhenScheduledToNextOccur = newOccurrence.WhenRescheduledToOccur.Value,
                            OccurrenceId = newOccurrence.Id,
                            SeriesId = item.Item2.Id,
                            Description = item.Item2.Description ?? "Unknown reminder",
                            Theme = item.Item2.ReminderTheme.ToDescription(),
                            TransactionAmount = item.Item2.Amount,
                            FromAccountName = item.Item2.TransactionFromAccount?.Nickname ?? "Unknown account",
                            RecurrenceDescription = null,
                            FromAccountTheme = item.Item2.TransactionFromAccount?.FinancialAccountTheme.ToDescription(),
                            NextRecurrenceDetails = new HtmlString(GetUpcomingRecurrenceDetailsByDateRange(whenSearchBegins, newOccurrence.WhenRescheduledToOccur.Value))
                        };
                    }
                }
                else
                {
                    // Grab the reminder's data
                    row = new ReminderItem
                    {
                        // This should never be null if we have a cause of edit
                        WhenScheduledToNextOccur = item.Item1,
                        SeriesId = item.Item2.Id,
                        Description = item.Item2.Description ?? "Unknown reminder",
                        Theme = item.Item2.ReminderTheme.ToDescription(),
                        TransactionAmount = item.Item2.Amount,
                        FromAccountName = item.Item2.TransactionFromAccount?.Nickname ?? "Unknown account",
                        RecurrenceDescription = null,
                        FromAccountTheme = item.Item2.TransactionFromAccount?.FinancialAccountTheme.ToDescription(),
                        NextRecurrenceDetails = new HtmlString(GetUpcomingRecurrenceDetailsByDateRange(whenSearchBegins, item.Item1))
                    };
                }

                filteredOccurences.Add(row);
            }

            return filteredOccurences;
        }

        private string GetTransactionCategory(string? nameOfReceivingAccount = null)
        {
            if (nameOfReceivingAccount == null)
            {
                return "Uncategorized";
            }

            return nameOfReceivingAccount;
        }

        private string GetUpcomingRecurrenceDetailsByDateRange(DateOnly whenBegins, DateOnly whenEnds)
        {
            var nextRecurrenceDetails = "";
            var difference = whenEnds.DayNumber - whenBegins.DayNumber;

            switch (difference)
            {
                case 0:
                    nextRecurrenceDetails = "today";
                    break;
                case 1:
                    nextRecurrenceDetails = "tomorrow";
                    break;
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    nextRecurrenceDetails = string.Format("in {0} days", difference);
                    break;
                case > 364:
                    nextRecurrenceDetails = whenEnds.ToString("on MMM d, yyyy");
                    break;
                default:
                    nextRecurrenceDetails = whenEnds.ToString("on MMM d");
                    break;
            }

            return nextRecurrenceDetails;
        }

        private void UpdateGlobalPageProperties()
        {
            _pageStateService.SetPageProperties(PAGE_TITLE);
        }

        private void HandleDayRangeChange(ChangeEventArgs e)
        {
            var selectedRange = e.Value;
            if (selectedRange != null && !string.IsNullOrWhiteSpace(selectedRange.ToString()))
            {
                _rangeInDays = selectedRange.ToString();
            }
        }

        private async Task HandleHideReminderModalClickAsync()
        {
            await SeriesModal.HideAsync();
        }

        private async Task HandleAddReminderButtonClickAsync()
        {
            CurrentSeries = new Reminder();
            CurrentSeries.WhenBegins = DateOnly.FromDateTime(DateTime.Now);
            await SeriesModal.ShowAsync();
        }

        private async Task HandleSaveReminderClickAsync()
        {
            _reminders = await GetRemindersAsync();

            SetReminderItems();

            await SeriesModal.HideAsync();
        }

        private void NavDemo()
        {
            //if (accountId != null && accountId.ToString() != string.Empty)
            //{
            //    _navigationManager.NavigateTo("/predictions/" + accountId.ToString());
            //    PredictionRows = await GetPredictionRowsAsync();
            //}

        }
        private async Task<IList<ReminderItem>> GetReminderRowsAsync()
        {
            // Get the list of reminders that we will use throughout the page's life
            var rows = new List<ReminderItem>();
            var query = new GetRemindersWithFullDetailsQuery();
            var reminders = await _queries.Execute(query);

            if (reminders == null)
            {
                return new List<ReminderItem>();
            }

            return reminders.Select(r => MapReminderToRecurringReminderRow(r)).ToList();
        }

        private ReminderItem MapReminderToRecurringReminderRow(Reminder reminder)
        {
            // Set the date variables that will be used throughout the code
            var today = DateOnly.FromDateTime(DateTime.Now);
            var whenNextRecurs = new DateOnly();
            DateOnly whenFirstOccurs = reminder.WhenBegins;
            DateOnly? whenExpires = reminder.WhenExpires;

            // If it falls within today and activeRangeInDays from now, we want to make at least one row for it
            if (whenExpires == null || whenExpires >= today)
            {
                // Only add those that start today or before and end after today
                // Get the recurrences
                if (reminder.RecurrenceTheme == RecurrenceStyle.NoRecurrence && whenFirstOccurs > today)
                {
                    // No recurrence - single occurence
                    whenNextRecurs = whenFirstOccurs;
                }
                else
                {
                    if (reminder.RecurrenceTheme == RecurrenceStyle.Day)
                    {
                        // Daily recurrences
                        // Start the loop at the first occurance and run until our running date is less than or equal to today.
                        // Collect the occurence dates
                        var whenRecurs = new List<DateOnly>();

                        for (DateOnly runningDate = whenFirstOccurs; runningDate <= today; runningDate = runningDate.AddDays(reminder.RecurrenceInterval))
                        {
                            whenRecurs.Add(runningDate);
                        }

                        if (whenRecurs.Any())
                        {
                            // Get the last recurrence and run it again against the interval to get the next recurrence
                            var whenLastRecurred = whenRecurs.LastOrDefault();
                            whenNextRecurs = whenLastRecurred.AddDays(reminder.RecurrenceInterval);
                        }
                        else
                        {
                            // If it has never occurred, it's first run will be the next
                            whenNextRecurs = whenFirstOccurs;
                        }

                    }
                    else if (reminder.RecurrenceTheme == RecurrenceStyle.Week)
                    {
                        // Weekly recurrences
                        // RecurrenceInterval X 7 days in a week
                        var daysBetweenRecurrences = reminder.RecurrenceInterval * 7;
                        // Start the loop at the first occurance and run until our running date is less than or equal to today.
                        // Collect the occurence dates
                        var whenRecurs = new List<DateOnly>();

                        for (DateOnly runningDate = whenFirstOccurs; runningDate <= today; runningDate = runningDate.AddDays(daysBetweenRecurrences))
                        {
                            whenRecurs.Add(runningDate);
                        }

                        if (whenRecurs.Any())
                        {
                            // Get the last recurrence and run it again against the interval to get the next recurrence
                            var whenLastRecurred = whenRecurs.LastOrDefault();
                            whenNextRecurs = whenLastRecurred.AddDays(reminder.RecurrenceInterval);
                        }
                        else
                        {
                            // If it has never occurred, it's first run will be the next
                            whenNextRecurs = whenFirstOccurs;
                        }
                    }
                    else if (reminder.RecurrenceTheme == RecurrenceStyle.Month)
                    {
                        // Monthly recurrences
                        // Start the loop at the first occurance and run until our running date is less than or equal to today.
                        // Collect the occurence dates
                        var whenRecurs = new List<DateOnly>();

                        for (DateOnly runningDate = whenFirstOccurs; runningDate <= today; runningDate = runningDate.AddMonths(reminder.RecurrenceInterval))
                        {
                            whenRecurs.Add(runningDate);
                        }

                        if (whenRecurs.Any())
                        {
                            // Get the last recurrence and run it again against the interval to get the next recurrence
                            var whenLastRecurred = whenRecurs.LastOrDefault();
                            whenNextRecurs = whenLastRecurred.AddMonths(reminder.RecurrenceInterval);
                        }
                        else
                        {
                            // If it has never occurred, it's first run will be the next
                            whenNextRecurs = whenFirstOccurs;
                        }
                    }
                }
            }

            var row = new ReminderItem
            {
                SeriesId = reminder.Id,
                Description = reminder.Description ?? "Unknown reminder",
                Theme = reminder.ReminderTheme.ToDescription(),
                TransactionAmount = reminder.Amount,
                FromAccountName = reminder.TransactionFromAccount?.Nickname ?? "Unknown account",
                TransactionCategory = GetTransactionCategory(reminder.TransactionToAccount?.Nickname),
                RecurrenceDescription = GetRecurrenceDescription(reminder),
                FromAccountTheme = reminder.TransactionFromAccount?.FinancialAccountTheme.ToDescription(),
            };

            // Get the next recurrence's details
            row.NextRecurrenceDetails = new HtmlString(string.Format("Next: {0} on {1}", row.Cost, whenNextRecurs.ToString("MMM d, yyyy")));

            return row;
        }

        private string GetDateRangeText()
        {
            return string.Format("{0} - {1}", _dateRange.Item1.ToString("MMM d, yyyy"), _dateRange.Item2.ToString("MMM d, yyyy"));

        }
        private string GetRecurrenceDescription(Reminder reminder)
        {
            var description = "Occurs once";
            var doesRecur = false;

            switch (reminder.RecurrenceTheme)
            {
                case RecurrenceStyle.Day:
                    description = "day";
                    break;
                case RecurrenceStyle.Week:
                    description = "week";
                    break;
                case RecurrenceStyle.Month:
                    description = "month";
                    break;
                default: // No recurrence/single occurance
                    description = "Occurs once";
                    break;
            }

            if (reminder.RecurrenceTheme != RecurrenceStyle.NoRecurrence)
            {
                if (reminder.RecurrenceInterval > 1)
                {
                    description = string.Format("Every {0} {1}s", reminder.RecurrenceInterval, description);
                }
                else
                {
                    description = string.Format("Every {0}", description);
                }
            }

            return description;

        }

        private IList<ButtonStateContainer> GetRecurringRemindersListFilterButtons()
        {
            var buttons = new List<ButtonStateContainer>();

            buttons.Add(new ButtonStateContainer
            {
                Text = "All Active",
                Value = "all",
                IsActive = true
            });

            buttons.Add(new ButtonStateContainer
            {
                Text = "Bills",
                Value = ReminderStyle.Bill.ToDescription().ToLower(),
                IsActive = false
            });

            buttons.Add(new ButtonStateContainer
            {
                Text = "Income",
                Value = ReminderStyle.Income.ToDescription().ToLower(),
                IsActive = false
            });

            buttons.Add(new ButtonStateContainer
            {
                Text = "Transfers",
                Value = ReminderStyle.Transfer.ToDescription().ToLower(),
                IsActive = false
            });

            return buttons;
        }

        private void HandleFilterClick(ButtonStateContainer selectedFilter)
        {
            if (RecurringReminderFilterButtons != null && RecurringReminderFilterButtons.Any())
            {
                foreach (var button in RecurringReminderFilterButtons)
                {
                    if (button.Value == selectedFilter.Value)
                    {
                        button.IsActive = true;
                        FilteredRecurringReminders = FilterRecurringReminders(button.Value);
                    }
                    else
                    {
                        button.IsActive = false;
                    }
                }
            }
        }

        private ReminderSummary GetReminderSummary()
        {
            if (_upcomingReminders == null)
            {
                return new ReminderSummary();
            }

            decimal income = _upcomingReminders.Where(r => r.Theme == ReminderStyle.Income.ToDescription()).Sum(r => r.TransactionAmount);
            decimal expenses = _upcomingReminders.Where(r => r.Theme == ReminderStyle.Bill.ToDescription()).Sum(r => r.TransactionAmount);

            return new ReminderSummary
            {
                Income = income,
                Expenses = expenses,
            };
        }

        private async Task HandleEditSeriesClickAsync(ReminderItem item)
        {

            CurrentSeries = _reminders.FirstOrDefault(r => r.Id == item.SeriesId);

            if (CurrentSeries == null)
            {
                await HandleAddReminderButtonClickAsync();
                return;
            }

            await SeriesModal.ShowAsync();
        }

        private void HandleViewSeriesClick(ReminderItem item)
        {

        }

        private void HandleDeleteSeriesClick(ReminderItem item)
        {

        }

        private void HandleCancelSeriesClick(ReminderItem item)
        {

        }

        private void UpdateReminder(ReminderItem item)
        {
            foreach (var reminder in _reminders)
            {
            }
        }

        private void SetReminderItems()
        {
            FilteredRecurringReminders = FilterRecurringReminders("all");
            RecurringReminderFilterButtons = GetRecurringRemindersListFilterButtons();
            _upcomingReminders = GetUpcomingReminders();
            Summary = GetReminderSummary();
        }

        private class ButtonStateContainer
        {
            public string Text { get; set; } = "Text";
            public string Value { get; set; } = "Value";
            public string CssClass => GetButtonStateCssClass();
            public bool IsActive { get; set; }

            string GetButtonStateCssClass()
            {
                var cssClass = "nav-link";

                if (IsActive)
                {
                    cssClass += " active";
                }

                return cssClass;
            }
        }

        private class ReminderSummary
        {
            public decimal Income { get; set; } = 0;
            public decimal Expenses { get; set; } = 0;
            public decimal NetTotal => GetNetTotal();
            private decimal GetNetTotal()
            {
                return Income - Expenses;
            }
        }

        public class ReminderItem
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public int SeriesId { get; set; }
            public int? OccurrenceId { get; set; }
            public string? RecurrenceDescription { get; set; }
            public HtmlString? NextRecurrenceDetails { get; set; }
            public string Cost => string.Format(CURRENCY_FORMAT, TransactionAmount);
            public string Description { get; set; } = "Unknown reminder";
            public string? Theme { get; set; }
            public string FromAccountName { get; set; } = "Unknown account";
            public string? TransactionCategory { get; set; }
            public string? FromAccountTheme { get; set; }
            public DateOnly WhenScheduledToNextOccur { get; set; }
            public DateOnly WhenScheduledToFirstOccur { get; set; }
            public Decimal TransactionAmount { get; set; } = 0;

        }

        private class OccurrenceForm : ReminderOccurrence
        {
        }

    }
}