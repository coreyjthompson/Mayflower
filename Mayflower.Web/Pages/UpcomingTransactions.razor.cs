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
    public partial class UpcomingTransactions
    {

        private const string PAGE_TITLE = "Bills & Payments";

        private string? _rangeInDays = "30";
        protected override void OnInitialized()
        {
            UpdateHeader();
        }

        private void UpdateHeader()
        {
            _headerChangeNotificationService.SetTitle(PAGE_TITLE);
        }

        private void HandleDayRangeChange(ChangeEventArgs e)
        {
            var selectedRange = e.Value;
            if (selectedRange != null && !string.IsNullOrWhiteSpace(selectedRange.ToString()))
            {
                _rangeInDays = selectedRange.ToString();
            }
        }

    }
}