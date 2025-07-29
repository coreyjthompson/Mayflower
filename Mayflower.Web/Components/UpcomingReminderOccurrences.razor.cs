using BlazorBootstrap;
using global::Microsoft.AspNetCore.Components;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Extensions;
using Mayflower.Core.Infrastructure.Commands;
using Mayflower.Core.Infrastructure.Commands.ReminderOccurences;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Mayflower.Core.Infrastructure.Queries;
using Mayflower.Core.Infrastructure.Queries.Reminders;

namespace Mayflower.Web.Components
{
    public partial class UpcomingReminderOccurrences
    {
        private const string CURRENCY_FORMAT = "#,##0.00";
        private const string EDIT_SKIP_ACTION_NAME = "edit-skip";
        private const string INSERT_SKIP_ACTION_NAME = "insert-skip";
        private const string COMPLETE_ACTION_NAME = "complete";
        private const string EDIT_REMINDER_ACTION_NAME = "edit-reminder";
        private const string EDIT_OCCURRENCE_ACTION_NAME = "edit-occurrence";
        private const string INSERT_OCCURRENCE_ACTION_NAME = "insert-occurrence";

        private bool _showPredictions = false;
        private int _currentRangeInDays = 30;
        private IList<Reminder> _reminders = new List<Reminder>();
        private IList<FinancialAccount>? _accounts = null;
        private IList<PredictionRow>? _predictionRows = null;
        private decimal _availableBalanace = 0;
        private Modal _editOccurrenceModal = new Modal();
        private OccurrenceForm _editOccurrenceForm = new OccurrenceForm();

        [Parameter]
        public string? RangeInDays { get; set; }
        protected override async Task OnInitializedAsync()
        {
            if(!string.IsNullOrWhiteSpace(RangeInDays))
            {
                int.TryParse(RangeInDays, out _currentRangeInDays);
            }
            _predictionRows = await GetPredictionRowsAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if (!string.IsNullOrWhiteSpace(RangeInDays))
            {
                int.TryParse(RangeInDays, out _currentRangeInDays);
            }

            _predictionRows = await GetPredictionRowsAsync();
        }

        private async Task<IList<PredictionRow>> GetPredictionRowsAsync()
        {
            // Get the list of reminders that we will use throughout the page's life
            var query = new GetAllRemindersWithOccurrencesQuery();
            _reminders = await _queries.Execute(query);
            // Sort them by date before any work is done on them
            var sortedReminders = _reminders.OrderBy(r => r.WhenBegins).ToList();

            var predictions = new List<PredictionRow>();
            var startDate = DateOnly.FromDateTime(DateTime.Now);
            var endDate = startDate.AddDays(this._currentRangeInDays);

            foreach (var reminder in sortedReminders)
            {
                // Set the date variables that will be used throughout the code
                DateOnly whenOccurs = reminder.WhenBegins;
                DateOnly? whenExpires = reminder.WhenExpires;
                // If it falls within today and activeRangeInDays from now, we want to make at least one row for it
                if ((whenOccurs <= startDate || (whenOccurs > startDate && whenOccurs <= endDate)) && (whenExpires == null || whenExpires >= endDate))
                {
                    // Only add those that start today or before and end after today
                    // Get the recurrences
                    if (reminder.RecurrenceTheme == RecurrenceStyle.NoRecurrence)
                    {
                        // No recurrence - single occurence
                        var row = MapBasicReminderDetailsToPredictionRow(reminder);
                        row.WhenScheduledToOccur = reminder.WhenBegins;
                        predictions.Add(row);
                    }
                    else
                    {
                        if (reminder.RecurrenceTheme == RecurrenceStyle.Day)
                        {
                            // Daily recurrences
                            // Start the loop at whenOccurs and run until our running date is equal to or greater than the end date.
                            // The running date will be our occurrence's display date
                            for (DateOnly runningDate = whenOccurs; runningDate <= endDate; runningDate = runningDate.AddDays(reminder.RecurrenceInterval))
                            {
                                var row = MapBasicReminderDetailsToPredictionRow(reminder);
                                row.WhenScheduledToOccur = runningDate;
                                predictions.Add(row);
                            }
                        }
                        else if (reminder.RecurrenceTheme == RecurrenceStyle.Week)
                        {
                            // Weekly recurrences
                            //RecurrenceInterval X 7 days in a week
                            var daysTillNextOccurrence = reminder.RecurrenceInterval * 7;
                            // Start the loop at whenOccurs and run until our running date is equal to or greater than the end date.
                            // The running date will be our occurrence's display date
                            for (DateOnly runningDate = whenOccurs; runningDate <= endDate; runningDate = runningDate.AddDays(daysTillNextOccurrence))
                            {
                                // If this new occurence is between start and end date, add it
                                if (runningDate >= startDate)
                                {
                                    var row = MapBasicReminderDetailsToPredictionRow(reminder);
                                    row.WhenScheduledToOccur = runningDate;
                                    predictions.Add(row);
                                }
                            }
                        }
                        else if (reminder.RecurrenceTheme == RecurrenceStyle.Month)
                        {
                            // Monthly recurrences
                            // Start the loop at whenOccurs and run until our running date is equal to or greater than the end date.
                            // The running date will be our occurrence's display date
                            for (DateOnly runningDate = whenOccurs; runningDate <= endDate; runningDate = runningDate.AddMonths(reminder.RecurrenceInterval))
                            {
                                // If this new occurence is between start and end date, add it
                                if (runningDate >= startDate)
                                {
                                    var row = MapBasicReminderDetailsToPredictionRow(reminder);
                                    row.WhenScheduledToOccur = runningDate;
                                    predictions.Add(row);
                                }
                            }
                        }
                    }
                }
            }

            // Sort them by date so we can properly calculate them in order
            predictions = predictions.OrderBy(r => r.WhenScheduledToOccur).ToList();
            var runningBalance = _availableBalanace;
            var filteredPredictions = new List<PredictionRow>();

            // Loop each row, filter them and setup menus by occurrence and sum it all up
            foreach (var row in predictions)
            {
                var showReminder = false;
                var occurence = row.OtherReminderOccurrences?.FirstOrDefault(o => o.WhenOriginallyScheduledToOccur == row.WhenScheduledToOccur);
                if (occurence != null)
                {
                    if (occurence.ReasonForOccurrence == ReminderOccurrenceCause.Edit)
                    {
                        // Grab the occurence's data
                        row.WhenScheduledToOccur = occurence.WhenRescheduledToOccur.Value; // This should never be null if we have a cause of edit
                        row.TransactionAmountForMath = occurence.Amount ?? 0;
                        row.OccurrenceId = occurence.Id;
                        // Now that it's already inserted, we want to change the button's action
                        row.ActionMenu.EditOccurenceButtonActionName = EDIT_OCCURRENCE_ACTION_NAME;
                        row.ActionMenu.SkipButtonActionName = EDIT_SKIP_ACTION_NAME;
                        // Show them all except the reminder edit
                        row.ActionMenu.ShowSkipButton = true;
                        row.ActionMenu.ShowEditOccurrenceButton = true;
                        row.ActionMenu.ShowEditReminderButton = false;
                        row.ActionMenu.EditOccurenceButtonText = "Edit this occurrence";
                        showReminder = true;
                    }
                }
                else
                {
                    row.ActionMenu.ShowAll();
                    showReminder = true;
                }

                if (showReminder)
                {
                    decimal transactionAmount = row.TransactionAmountForMath;
                    // Add or remove money from the running balance and set up the corresponding styles
                    if (row.ReminderTheme == ReminderStyle.Bill)
                    {
                        row.TransactionAmountCss = "outgoing";
                        if (_showPredictions)
                        {
                            //runningBalance = runningBalance - transactionAmount;
                            row.ClosingBalanceForDisplay = runningBalance.ToString(CURRENCY_FORMAT);
                            row.TransactionAmountForDisplay = "-" + transactionAmount.ToString(CURRENCY_FORMAT);
                        }
                        else
                        {
                            row.TransactionAmountForDisplay = transactionAmount.ToString(CURRENCY_FORMAT);
                        }

                    }
                    else
                    {
                        //runningBalance = runningBalance + transactionAmount;
                        row.ClosingBalanceForDisplay = runningBalance.ToString(CURRENCY_FORMAT);
                        row.TransactionAmountCss = "incoming";
                        row.TransactionAmountForDisplay = "+" + transactionAmount.ToString(CURRENCY_FORMAT);
                    }

                    filteredPredictions.Add(row);
                }
            }

            return filteredPredictions;
        }

        private async Task SetOccurenceFormData(PredictionRow row, string formAction)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == row.ReminderId);

            if (reminder != null)
            {
                _editOccurrenceForm = new OccurrenceForm
                {
                    Amount = row.TransactionAmountForMath,
                    WhenRescheduledToOccur = row.WhenScheduledToOccur,
                    ReminderId = row.ReminderId,
                    Id = row.OccurrenceId ?? 0,
                    Action = formAction
                };
            }

            // Satisfy the method and force a refresh of the ui
            await Task.FromResult(reminder);
        }

        private PredictionRow MapBasicReminderDetailsToPredictionRow(Reminder reminder)
        {
            var row = new PredictionRow
            {
                ReminderId = reminder.Id,
                WhenScheduledToOccur = reminder.WhenBegins,
                Description = reminder.Description,
                ReminderTheme = reminder.ReminderTheme,
                TransactionFromAccountId = reminder.TransactionFromAccountId,
                TransactionToAccountId = reminder.TransactionToAccountId,
                TransactionAmountForMath = reminder.Amount,
                OtherReminderOccurrences = reminder.Occurrences
            };
            return row;
        }

        private bool HasActiveScheduleWithinRange(Reminder reminder)
        {
            var start = DateOnly.FromDateTime(DateTime.Now);
            var end = start.AddDays(this._currentRangeInDays);
            // If it falls within today and activeRangeInDays from now, return true
            if (reminder.WhenBegins <= start && (reminder.WhenExpires == null || reminder.WhenExpires > end))
            {
                return true;
            }

            return false;
        }

        private async Task HandleActionLinkClickAsync(string action, PredictionRow row)
        {
            switch (action)
            {
                case INSERT_SKIP_ACTION_NAME:
                    await SkipOccurrenceAsync(row.ReminderId, INSERT_SKIP_ACTION_NAME);
                    break;
                case EDIT_SKIP_ACTION_NAME:
                    await SkipOccurrenceAsync(row.ReminderId, EDIT_SKIP_ACTION_NAME);
                    break;
                case COMPLETE_ACTION_NAME:
                    await CompleteReminderOccurrencesAsync(row.ReminderId);
                    break;
                case EDIT_OCCURRENCE_ACTION_NAME:
                    await SetOccurenceFormData(row, EDIT_OCCURRENCE_ACTION_NAME);
                    await _editOccurrenceModal.ShowAsync();
                    break;
                case INSERT_OCCURRENCE_ACTION_NAME:
                    await SetOccurenceFormData(row, INSERT_OCCURRENCE_ACTION_NAME);
                    await _editOccurrenceModal.ShowAsync();
                    break;
                case EDIT_REMINDER_ACTION_NAME:
                    await EditReminderAsync(row.ReminderId);
                    break;
                default:
                    break;
            }

            _predictionRows = await GetPredictionRowsAsync();
        }

        private async Task HandleOccurrenceFormSubmitAsync()
        {
            var row = _predictionRows?.FirstOrDefault(p => p.ReminderId == _editOccurrenceForm.ReminderId);

            if (row != null)
            {
                ICommand<bool>? command = null;

                if (_editOccurrenceForm.Action == EDIT_OCCURRENCE_ACTION_NAME)
                {
                    command = new EditReminderOccurenceCommand
                    {
                        Id = _editOccurrenceForm.Id,
                        ReasonForOccurrence = ReminderOccurrenceCause.Edit,
                        WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur,
                        WhenRescheduledToOccur = _editOccurrenceForm.WhenRescheduledToOccur,
                        Amount = _editOccurrenceForm.Amount
                    };

                }
                else if (_editOccurrenceForm.Action == INSERT_OCCURRENCE_ACTION_NAME)
                {
                    command = new InsertReminderOccurenceCommand
                    {
                        ReminderId = _editOccurrenceForm.ReminderId,
                        ReasonForOccurrence = ReminderOccurrenceCause.Edit,
                        WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur,
                        WhenRescheduledToOccur = _editOccurrenceForm.WhenRescheduledToOccur,
                        Amount = _editOccurrenceForm.Amount
                    };
                }

                if (command != null && await _commands.Execute(command))
                {
                    _predictionRows = await GetPredictionRowsAsync();

                    await _editOccurrenceModal.HideAsync();
                }
            }
        }

        private async Task HandleHideEditModalClickAsync()
        {
            await _editOccurrenceModal.HideAsync();
        }

        private async Task SkipOccurrenceAsync(int reminderId, string crudAction)
        {
            var row = _predictionRows?.FirstOrDefault(p => p.ReminderId == reminderId);

            if (row != null)
            {
                ICommand<bool>? command = null;

                if (crudAction == INSERT_SKIP_ACTION_NAME)
                {
                    command = new InsertReminderOccurenceCommand
                    {
                        ReminderId = reminderId,
                        ReasonForOccurrence = ReminderOccurrenceCause.Skip,
                        WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur
                    };
                }
                else if (crudAction != EDIT_SKIP_ACTION_NAME)
                {
                    command = new EditReminderOccurenceCommand
                    {
                        Id = row.OccurrenceId ?? 0,
                        ReasonForOccurrence = ReminderOccurrenceCause.Skip
                    };
                }

                if (command != null)
                {
                    await _commands.Execute(command);
                }
            }
        }

        private async Task EditReminderAsync(int id)
        {
        }

        private async Task CompleteReminderOccurrencesAsync(int id)
        {
        }

        private class PredictionRow
        {
            public Guid Id { get; set; } = Guid.NewGuid();
            public int ReminderId { get; set; }
            public int? OccurrenceId { get; set; }
            public DateOnly WhenScheduledToOccur { get; set; }
            public string TransactionAmountForDisplay { get; set; } = (0).ToString(CURRENCY_FORMAT);
            public decimal TransactionAmountForMath { get; set; }
            public string? Description { get; set; }
            public ReminderStyle ReminderTheme { get; set; }
            public string ClosingBalanceForDisplay { get; set; } = (0).ToString(CURRENCY_FORMAT);
            public string? TransactionAmountCss { get; set; }
            public string? ClosingBalanceCss { get; set; }
            public int? TransactionFromAccountId { get; set; }
            public int? TransactionToAccountId { get; set; }
            public IList<ReminderOccurrence>? OtherReminderOccurrences { get; set; }
            public ActionMenu ActionMenu { get; set; } = new ActionMenu();
        }

        private class ActionMenu
        {
            public string SkipButtonText { get; set; } = "Skip this occurence";
            public string CompleteButtonText { get; set; } = "Complete this occurence";
            public string EditOccurenceButtonText { get; set; } = "Edit this occurence only";
            public string EditReminderButtonText { get; set; } = "Edit this and every occurrence after";
            public bool ShowSkipButton { get; set; }
            public bool ShowCompleteButton { get; set; }
            public bool ShowEditOccurrenceButton { get; set; }
            public bool ShowEditReminderButton { get; set; }
            public bool ShowGroupDivider1 => (ShowSkipButton || ShowCompleteButton) && (ShowEditOccurrenceButton || ShowEditReminderButton);
            public bool ShowMenu => ShowSkipButton || ShowCompleteButton || ShowEditOccurrenceButton || ShowEditReminderButton;
            public string SkipButtonActionName { get; set; } = INSERT_SKIP_ACTION_NAME;
            public string CompleteButtonActionName { get; set; } = COMPLETE_ACTION_NAME;
            public string EditOccurenceButtonActionName { get; set; } = INSERT_OCCURRENCE_ACTION_NAME;
            public string EditReminderButtonActionName { get; set; } = EDIT_REMINDER_ACTION_NAME;

            public void ShowAll()
            {
                ShowSkipButton = true;
                //ShowCompleteButton = true;
                ShowEditOccurrenceButton = true;
                ShowEditReminderButton = true;
            }
        }

        private class OccurrenceForm : ReminderOccurrence
        {
            public string Action { get; set; } = INSERT_OCCURRENCE_ACTION_NAME;
        }

    }
}