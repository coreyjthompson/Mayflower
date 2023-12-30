using Microsoft.AspNetCore.Components;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Queries;
using Mayflower.Core.Infrastructure.Queries.Reminders;
using Mayflower.Core.Extensions;
using BlazorBootstrap;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace Mayflower.Web.Pages
{
    public partial class FetchReminders
    {
        private const string CURRENCYFORMAT = "#,##0.00";
        private bool _updateReminders = false;
        private IList<Reminder> _reminders = new List<Reminder>();

        private IList<FinancialAccount>? Accounts { get; set; } = null;
        private IList<ReminderRow>? ReminderRows { get; set; } = null;
        private ReminderForm EditReminderForm { get; set; } = new ReminderForm();
        private Modal ReminderFormModal { get; set; } = default!;

        [Parameter]
        public string? AccountId { get; set; }
        protected Reminder SelectedReminder { get; set; } = new Reminder();

        protected override async Task OnInitializedAsync()
        {
            Accounts = await GetAccountsAsync();
            ReminderRows = await GetReminderRowsAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            if(_updateReminders)
            {
                ReminderRows = await GetReminderRowsAsync();
                _updateReminders = false;
            }
        }

        private async Task SetRemindersAsync()
        {
            var accountId = GetConvertedAccountId();

            if (accountId != 0)
            {
                var query = new GetAllRemindersByFinancialAccountIdQuery(accountId);
                _reminders = await _queries.Execute(query);
                // Sort them by date 
                _reminders = _reminders.OrderBy(r => r.WhenBegins).ToList();
            }
        }

        private async Task<IList<FinancialAccount>> GetAccountsAsync()
        {
            var query = new GetFinancialAccountsQuery();
            return await _queries.Execute(query);
        }

        private async Task<IList<ReminderRow>> GetReminderRowsAsync()
        {
            await SetRemindersAsync();

            var reminderRows = new List<ReminderRow>();
            // Loop each row and sum it all up 
            foreach (var reminder in _reminders)
            {
                var row = MapReminderDetailsToTableRow(reminder);

                reminderRows.Add(row);
            }

            return reminderRows;
        }

        private async Task HandleAccountChangeAsync(ChangeEventArgs e)
        {
            var accountId = e.Value;
            _updateReminders = true;

            if (accountId != null && accountId.ToString() != string.Empty)
            {
                AccountId = accountId.ToString();
                _navigationManager.NavigateTo("/reminders/" + AccountId);
                ReminderRows = await GetReminderRowsAsync();
            }
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

        private ReminderRow MapReminderDetailsToTableRow(Reminder reminder)
        {
            var accountId = GetConvertedAccountId();
            var row = new ReminderRow
            {
                Id = reminder.Id,
                WhenOccurs = reminder.WhenBegins,
                Description = reminder.Description,
                ReminderTheme = reminder.ReminderTheme.ToDescription() ?? string.Empty,
                TransactionFromAccountId = reminder.TransactionFromAccountId,
                TransactionToAccountId = reminder.TransactionToAccountId,
                Amount = reminder.Amount
            };
            if (row.TransactionFromAccountId == accountId)
            {
                row.AmountCss = "outgoing";
                row.AmountForDisplay = "-" + reminder.Amount.ToString(CURRENCYFORMAT);
            }
            else
            {
                row.AmountCss = "incoming";
                row.AmountForDisplay = reminder.Amount.ToString(CURRENCYFORMAT);
            }

            return row;
        }

        private async Task HandleHideReminderModalClickAsync()
        {
            await ReminderFormModal.HideAsync();
        }

        private async Task HandleReminderFormSubmitAsync()
        {
            //var row = PredictionRows?.FirstOrDefault(p => p.ReminderId == EditOccurrenceForm.ReminderId);

            //if (row != null)
            //{
            //    ICommand<bool>? command = null;

            //    if (EditOccurrenceForm.Action == EDIT_OCCURRENCE_ACTION_NAME)
            //    {
            //        command = new EditReminderOccurenceCommand
            //        {
            //            Id = EditOccurrenceForm.Id,
            //            ReasonForOccurrence = ReminderOccurrenceCause.Edit,
            //            WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur,
            //            WhenRescheduledToOccur = EditOccurrenceForm.WhenRescheduledToOccur,
            //            Amount = EditOccurrenceForm.Amount
            //        };

            //    }
            //    else if (EditOccurrenceForm.Action == INSERT_OCCURRENCE_ACTION_NAME)
            //    {
            //        command = new InsertReminderOccurenceCommand
            //        {
            //            ReminderId = EditOccurrenceForm.ReminderId,
            //            ReasonForOccurrence = ReminderOccurrenceCause.Edit,
            //            WhenOriginallyScheduledToOccur = row.WhenScheduledToOccur,
            //            WhenRescheduledToOccur = EditOccurrenceForm.WhenRescheduledToOccur,
            //            Amount = EditOccurrenceForm.Amount
            //        };
            //    }

            //    if (command != null && await _commands.Execute(command))
            //    {
            //        PredictionRows = await GetPredictionRowsAsync();

            //        await EditOccurrenceModal.HideAsync();
            //    }
            //}
        }

        private async Task SetReminderFormData(int id)
        {
            var reminder = _reminders.FirstOrDefault(r => r.Id == id);

            if (reminder != null)
            {
                EditReminderForm = new ReminderForm
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

            // Satisfy the method and force a refresh of the ui
            await Task.FromResult(reminder);
        }

        private async Task HandleEditReminderButtonClick(int id)
        {
            if (id != 0)
            {
                var reminder = _reminders.FirstOrDefault(r => r.Id == id);
                if (reminder != null)
                {
                    SelectedReminder = reminder;
                }
            }

            await ReminderFormModal.ShowAsync();
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

        private IList<(RecurrencePosition, string, string)> GetPositions()
        {
            var list = new List<(RecurrencePosition, string, string)>();

            foreach(var item in Enum.GetValues(typeof(RecurrencePosition)))
            {
                var position = (RecurrencePosition)item;

                if(position != RecurrencePosition.None)
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

                if(style != ReminderStyle.Error)
                {
                    var description = style.ToDescription();

                    list.Add((style, ((int)style).ToString(), description));
                }

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

        private class ReminderRow
        {
            public int Id { get; set; }
            public DateOnly WhenOccurs { get; set; }
            public string AmountForDisplay { get; set; } = (0).ToString(CURRENCYFORMAT);
            public decimal Amount { get; set; }
            public string? Description { get; set; }
            public string ReminderTheme { get; set; } = default !;
            public string? AmountCss { get; set; }
            public int? TransactionFromAccountId { get; set; }
            public int? TransactionToAccountId { get; set; }
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
            public string WhenBeginsLabelText { get; set; } = "Next Due Date";
            public string WhenExpiresLabelText { get; set; } = default!;
            public string AmountLabelText { get; set; } = "Amount";
            public string ReminderThemeLabelText { get; set; } = "Type";
            public string DescriptionLabelText { get; set; } = "Description";
            public string TransactionToLabelText { get; set; } = "Select an account";
            public string TransactionFromLabelText { get; set; } = "Select an account";
            public string RecurrenceThemeLabelText { get; set; } = default!;
            public string RecurrenceIntervalLabelText { get; set; } = "Interval";
            public string RecurrenceDayOfMonthLabelText { get; set; } = "Day of the month";
            public string RecurrenceDayOfWeekLabelText { get; set; } = "Day of the week";
            public string RecurrenceOrdinalLabelText { get; set; } = "Ordinal";
            #endregion

            #region Placeholders
            public string WhenBeginsPlaceholderText { get; set; } = "Next Due Date";
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

    }
}