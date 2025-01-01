using Microsoft.AspNetCore.Components;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Queries;
using Mayflower.Core.Infrastructure.Queries.Reminders;
using Mayflower.Core.Extensions;
using BlazorBootstrap;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;
using Mayflower.Web.Shared;
using Microsoft.Identity.Client;

namespace Mayflower.Web.Pages
{
    public partial class Reminders
    {

        private const string PAGE_TITLE = "Bills & Payments";

        private string? _rangeInDays = "30";
        private Modal _reminderFormModal = new Modal();
        private Reminder _selectedReminder { get; set; } = new Reminder();

        protected override void OnInitialized()
        {
            UpdateHeader();
        }

        private void UpdateHeader()
        {
            _headerChangeNotificationService.SetPageProperties(PAGE_TITLE);
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
            await _reminderFormModal.HideAsync();
        }

        private async Task HandleAddReminderButtonClick()
        {
            _selectedReminder = new Reminder();
            _selectedReminder.WhenBegins = DateOnly.FromDateTime(DateTime.Now);
            await _reminderFormModal.ShowAsync();
        }

        private async Task HandleSaveReminderClickAsync()
        {
            // Add a new reminder

            await _reminderFormModal.HideAsync();
        }


    }
}