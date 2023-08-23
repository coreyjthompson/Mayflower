using global::Microsoft.AspNetCore.Components;
using Mayflower.Core.DomainModels;
using BlazorBootstrap;
using Mayflower.Core.Infrastructure.Queries;
using Mayflower.Core.Infrastructure.Queries.Reminders;
using Mayflower.Core.Infrastructure.Interfaces.Queries;
using Mayflower.Core.Infrastructure.Commands;
using Mayflower.Core.Infrastructure.Interfaces.Commands;

namespace Mayflower.Web.Pages;

public partial class FetchPredictions
{
    private const string CURRENCYFORMAT = "#,##0.00";
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
        await OnInitializedAsync();
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
            reminders = _reminders.OrderBy(r => r.WhenOccurs).ToList();
        }

        var predictions = new List<PredictionRow>();
        var startDate = DateOnly.FromDateTime(DateTime.Now);
        var endDate = startDate.AddDays(this._currentRangeInDays);
        foreach (var reminder in reminders)
        {
            // Set the date variables that will be used throughout the code
            DateOnly whenOccurs = reminder.WhenOccurs;
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
                    row.WhenScheduledToOccur = reminder.WhenOccurs;
                    predictions.Add(row);
                }
                else
                {
                    if (reminder.RecurrenceTheme == RecurrenceStyle.Daily)
                    {
                        // Daily recurrences
                        // Start the loop at whenOccurs and run until our
                        // running date is equal to or greater than the end date.
                        // The running date will be our occurrence's display date
                        for (DateOnly runningDate = whenOccurs; runningDate <= endDate; runningDate = runningDate.AddDays(reminder.RecurrenceInterval))
                        {
                            var row = MapBasicReminderDetailsToPredictionRow(reminder);
                            row.WhenScheduledToOccur = runningDate;
                            predictions.Add(row);
                        }
                    }
                    else if (reminder.RecurrenceTheme == RecurrenceStyle.Weekly)
                    {
                        // Weekly recurrences
                        //RecurrenceInterval X 7 days in a week
                        var daysTillNextOccurrence = reminder.RecurrenceInterval * 7;
                        // Start the loop at whenOccurs and run until our
                        // running date is equal to or greater than the end date.
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
                    else if (reminder.RecurrenceTheme == RecurrenceStyle.Monthly)
                    {
                        // Monthly recurrences
                        // Start the loop at whenOccurs and run until our
                        // running date is equal to or greater than the end date.
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
        // Loop each row, filter out those that have been logged and sum it all up
        foreach (var row in predictions)
        {
            var occurence = row.OtherReminderOccurrences?.FirstOrDefault(o => o.WhenOriginallyScheduledToOccur == row.WhenScheduledToOccur);
            if (occurence == null)
            {
                decimal transactionAmount = row.TransactionAmountForMath;
                // Add or remove money from the running balance and set up the corresponding styles
                if (row.TransactionFromAccountId == accountId)
                {
                    runningBalance = runningBalance - transactionAmount;
                    row.ClosingBalanceForDisplay = runningBalance.ToString(CURRENCYFORMAT);
                    row.TransactionAmountCss = "outgoing";
                    row.TransactionAmountForDisplay = "-" + transactionAmount.ToString(CURRENCYFORMAT);
                }
                else
                {
                    runningBalance = runningBalance + transactionAmount;
                    row.ClosingBalanceForDisplay = runningBalance.ToString(CURRENCYFORMAT);
                    row.TransactionAmountCss = "incoming";
                    row.TransactionAmountForDisplay = transactionAmount.ToString(CURRENCYFORMAT);
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
        var accountId = GetConvertedAccountId();
        var row = new PredictionRow
        {
            ReminderId = reminder.Id,
            WhenScheduledToOccur = reminder.WhenOccurs,
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
        if (reminder.WhenOccurs <= start && (reminder.WhenExpires == null || reminder.WhenExpires > end))
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
            case "skip":
                await SkipOccurrenceAsync(row.ReminderId);
                break;
            case "complete":
                await CompleteReminderOccurrencesAsync(row.ReminderId);
                break;
            case "edit-this":
                await EditOccurrenceAsync(row);
                await EditOccurrenceModal.ShowAsync();
                break;
            case "edit-all":
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

    private async Task HandleOccurrenceFormSubmitAsync()
    {
        var row = PredictionRows?.FirstOrDefault(p => p.ReminderId == EditOccurrenceForm.ReminderId);
        if (row != null)
        {
            var command = new InsertReminderOccurenceCommand
            {
                ReminderId = EditOccurrenceForm.ReminderId,
                ReasonForOccurrence = ReminderOccurrenceCause.Edit,
                WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur,
                WhenRescheduledToOccur = EditOccurrenceForm.WhenRescheduledToOccur
            };
            var isSuccessful = await _commands.Execute(command);
            if (isSuccessful)
            {
                PredictionRows = await GetPredictionRowsAsync();
                await EditOccurrenceModal.HideAsync();
            }
        }
    }

    private async Task SkipOccurrenceAsync(int reminderId)
    {
        var row = PredictionRows?.FirstOrDefault(p => p.ReminderId == reminderId);
        if (row != null)
        {
            var command = new InsertReminderOccurenceCommand
            {
                ReminderId = reminderId,
                ReasonForOccurrence = ReminderOccurrenceCause.Skip,
                WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur
            };
            await _commands.Execute(command);
        }
    }

    private async Task EditOccurrenceAsync(PredictionRow row)
    {
        var reminder = _reminders.FirstOrDefault(r => r.Id == row.ReminderId);
        if (reminder != null)
        {
            EditOccurrenceForm = new OccurrenceForm
            {
                Amount = reminder.Amount,
                WhenRescheduledToOccur = row.WhenScheduledToOccur,
                ReminderId = row.ReminderId
            };
        }

        // Satisfy the method and force a refresh of the ui
        await Task.FromResult(reminder);
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
        public DateOnly WhenScheduledToOccur { get; set; }
        public string TransactionAmountForDisplay { get; set; } = (0).ToString(CURRENCYFORMAT);
        public decimal TransactionAmountForMath { get; set; }
        public string? Description { get; set; }
        public ReminderStyle ReminderTheme { get; set; }
        public string ClosingBalanceForDisplay { get; set; } = (0).ToString(CURRENCYFORMAT);
        public string? TransactionAmountCss { get; set; }
        public string? ClosingBalanceCss { get; set; }
        public int? TransactionFromAccountId { get; set; }
        public int? TransactionToAccountId { get; set; }
        public IList<ReminderOccurrence>? OtherReminderOccurrences { get; set; }
    }

    private class OccurrenceForm : ReminderOccurrence
    {
    }
}