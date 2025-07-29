using BlazorBootstrap;
using global::Microsoft.AspNetCore.Components;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Commands.ReminderOccurences;
using Mayflower.Core.Infrastructure.Interfaces.Commands;
using Mayflower.Core.Infrastructure.Queries;
using Mayflower.Core.Infrastructure.Queries.Reminders;

namespace Mayflower.Web.Pages;

public partial class FetchPredictions
{
    protected const string CURRENCY_FORMAT = "#,##0.00";
    protected const string EDIT_SKIP_ACTION_NAME = "edit-skip";
    protected const string INSERT_SKIP_ACTION_NAME = "insert-skip";
    protected const string COMPLETE_ACTION_NAME = "complete";
    protected const string EDIT_REMINDER_ACTION_NAME = "edit-reminder";
    protected const string EDIT_OCCURRENCE_ACTION_NAME = "edit-occurrence";
    protected const string INSERT_OCCURRENCE_ACTION_NAME = "insert-occurrence";

    private int _currentRangeInDays = 60;
    private IList<Reminder> _reminders = new List<Reminder>();

    private IList<FinancialAccount>? Accounts { get; set; } = null;
    private IList<PredictionRow>? PredictionRows { get; set; } = null;
    private OccurrenceForm EditOccurrenceForm { get; set; } = new OccurrenceForm();
    private decimal AvailableBalance { get; set; } = 0;
    private Modal EditOccurrenceModal { get; set; } = default !;

    [Parameter]
    public string? AccountId { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Accounts = await GetAccountsAsync();
        await SetAvailableBalanceDetailsAsync();
        PredictionRows = await GetPredictionRowsAsync();
    }

    protected override async Task OnParametersSetAsync()
    {
        await SetAvailableBalanceDetailsAsync();
        PredictionRows = await GetPredictionRowsAsync();
    }

    private async Task<IList<PredictionRow>> GetPredictionRowsAsync()
    {
        var accountId = GetConvertedAccountId();
        var reminders = new List<Reminder>();
        if (accountId != 0)
        {
            // Get the list of reminders that we will use throughout the page's life
            var query = new GetAllRemindersByFinancialAccountIdQuery(accountId);
            _reminders = await _queries.Execute(query);
            // Sort them by date before any work is done on them
            reminders = _reminders.OrderBy(r => r.WhenBegins).ToList();
        }

        var predictions = new List<PredictionRow>();
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = startDate.AddDays(this._currentRangeInDays);

        foreach (var reminder in reminders)
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
        var runningBalance = this.AvailableBalance;
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
                if (row.TransactionFromAccountId == accountId)
                {
                    runningBalance = runningBalance - transactionAmount;
                    row.ClosingBalanceForDisplay = runningBalance.ToString(CURRENCY_FORMAT);
                    row.TransactionAmountCss = "outgoing";
                    row.TransactionAmountForDisplay = "-" + transactionAmount.ToString(CURRENCY_FORMAT);
                }
                else
                {
                    runningBalance = runningBalance + transactionAmount;
                    row.ClosingBalanceForDisplay = runningBalance.ToString(CURRENCY_FORMAT);
                    row.TransactionAmountCss = "incoming";
                    row.TransactionAmountForDisplay = transactionAmount.ToString(CURRENCY_FORMAT);
                }

                filteredPredictions.Add(row);
            }
        }

        return filteredPredictions;
    }

    private async Task SetAvailableBalanceDetailsAsync()
    {
        var accountId = GetConvertedAccountId();
        var query = new GetLatestStatementByAccountIdQuery();
        query.AccountId = accountId;
        FinancialStatement statement = await _queries.Execute(query);
        var amount = statement?.AvailableBalance?.Amount;
        var asOf = statement?.AvailableBalance?.AsOf;
        if (amount != null && asOf != null)
        {
            AvailableBalance = amount.Value;
        }
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

    private async Task HandleAccountChangeAsync(ChangeEventArgs e)
    {
        var accountId = e.Value;
        if (accountId != null && accountId.ToString() != string.Empty)
        {
            _navigationManager.NavigateTo("/predictions/" + accountId.ToString());
            PredictionRows = await GetPredictionRowsAsync();
        }
    }

    private async Task HandleCurrentFundsSubmitAsync()
    {
        PredictionRows = await GetPredictionRowsAsync();
    }

    private int GetConvertedAccountId()
    {
        int id;
        if (!string.IsNullOrWhiteSpace(AccountId) && int.TryParse(AccountId, out id))
        {
            return id;
        }

        return 0;
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

    private async Task<IList<FinancialAccount>> GetAccountsAsync()
    {
        var query = new GetFinancialAccountsQuery();
        return await _queries.Execute(query);
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
                await EditOccurrenceModal.ShowAsync();
                break;
            case INSERT_OCCURRENCE_ACTION_NAME:
                await SetOccurenceFormData(row, INSERT_OCCURRENCE_ACTION_NAME);
                await EditOccurrenceModal.ShowAsync();
                break;
            case EDIT_REMINDER_ACTION_NAME:
                await EditReminderAsync(row.ReminderId);
                break;
            default:
                break;
        }

        PredictionRows = await GetPredictionRowsAsync();
    }

    private async Task HandleHideEditModalClickAsync()
    {
        await EditOccurrenceModal.HideAsync();
    }

    private async Task SkipOccurrenceAsync(int reminderId, string crudAction)
    {
        var row = PredictionRows?.FirstOrDefault(p => p.ReminderId == reminderId);

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
            else if(crudAction != EDIT_SKIP_ACTION_NAME)
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

    private async Task SetOccurenceFormData(PredictionRow row, string formAction)
    {
        var reminder = _reminders.FirstOrDefault(r => r.Id == row.ReminderId);
        
        if (reminder != null)
        {
            EditOccurrenceForm = new OccurrenceForm
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

    private async Task HandleOccurrenceFormSubmitAsync()
    {
        var row = PredictionRows?.FirstOrDefault(p => p.ReminderId == EditOccurrenceForm.ReminderId);

        if (row != null)
        {
            ICommand<bool>? command = null;

            if (EditOccurrenceForm.Action == EDIT_OCCURRENCE_ACTION_NAME) 
            {
                command = new EditReminderOccurenceCommand
                {
                    Id = EditOccurrenceForm.Id,
                    ReasonForOccurrence = ReminderOccurrenceCause.Edit,
                    WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur,
                    WhenRescheduledToOccur = EditOccurrenceForm.WhenRescheduledToOccur,
                    Amount = EditOccurrenceForm.Amount
                };

            }
            else if (EditOccurrenceForm.Action == INSERT_OCCURRENCE_ACTION_NAME)
            {
                command = new InsertReminderOccurenceCommand
                {
                    ReminderId = EditOccurrenceForm.ReminderId,
                    ReasonForOccurrence = ReminderOccurrenceCause.Edit,
                    WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur,
                    WhenRescheduledToOccur = EditOccurrenceForm.WhenRescheduledToOccur,
                    Amount = EditOccurrenceForm.Amount
                };
            }

            if (command != null && await _commands.Execute(command))
            {
                PredictionRows = await GetPredictionRowsAsync();

                await EditOccurrenceModal.HideAsync();
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

    private class OccurrenceForm : ReminderOccurrence
    {
        public string Action { get; set; } = INSERT_OCCURRENCE_ACTION_NAME;
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
        public bool ShowMenu => ShowSkipButton || ShowCompleteButton ||ShowEditOccurrenceButton || ShowEditReminderButton;
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

}