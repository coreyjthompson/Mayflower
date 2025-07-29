using BlazorBootstrap;
using Mayflower.Core.DomainModels;
using Mayflower.Core.Infrastructure.Queries.Accounts;
using Mayflower.Web.Components;
using Microsoft.AspNetCore.Html;

namespace Mayflower.Web.Pages
{
    public partial class Accounts
    {

        private const string PAGE_TITLE = "Accounts";
        private IList<FinancialAccount> _financialAccounts = new List<FinancialAccount>();

        private IList<AccountRow> AccountRows = new List<AccountRow>();

        private FinancialAccountForm AccountModalForm { get; set; } = new FinancialAccountForm();
        private Modal AccountModal { get; set; } = new Modal();
        private FinancialAccount? SelectedAccount { get; set; } = new FinancialAccount();

        protected override async Task OnInitializedAsync()
        {
            UpdateGlobalPageProperties();

            AccountRows = await GetAccountRowsAsync();

            //SetAccountRows();
        }
        private void UpdateGlobalPageProperties()
        {
            _pageStateService.SetPageProperties(PAGE_TITLE);
        }

        private async Task HandleHideAccountModalClickAsync()
        {
            await AccountModal.HideAsync();
        }

        private async Task<IList<AccountRow>> GetAccountRowsAsync()
        {
            var query = new GetFinancialAccountsQuery();
            _financialAccounts = await _queries.Execute(query);

            if (_financialAccounts == null)
            {
                return new List<AccountRow>();
            }

            return _financialAccounts.Select(a => MapToAccountRow(a)).ToList();
        }

        private AccountRow MapToAccountRow(FinancialAccount account)
        {
            var accountType = account.FinancialAccountTheme.ToString();
            var maskedNumber = new HtmlString(MaskAccountNumber(account.Number));
            var maskedDetails = string.Format("{0} {1}", accountType, maskedNumber);

            var row = new AccountRow
            {
                Id = account.Id,
                Nickname = account.Nickname,
                Number = account.Number,
                TypeName = accountType,
                InstitutionId = account.FinancialInstitutionId,
                InstitutionName = account.FinancialInstitution.Name,
                AccountNumberDetails = new HtmlString(maskedDetails)
            };

            return row;
        }

        private string MaskAccountNumber(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return string.Empty;
            }
            var maskedNumber = number.Substring(Math.Max(0, number.Length - 4));

            return string.Format("••••{0}", maskedNumber);
        }

        private async Task HandleEditAccountClickAsync(AccountRow item)
        {
            SelectedAccount = _financialAccounts.FirstOrDefault(r => r.Id == item.Id);

            if (SelectedAccount == null)
            {
                await HandleAddAccountButtonClickAsync();
                return;
            }

            await AccountModal.ShowAsync();
        }
        private async Task HandleAddAccountButtonClickAsync()
        {
            SelectedAccount = new FinancialAccount();

            await AccountModal.ShowAsync();
        }

        private async Task HandleSaveAccountClickAsync()
        {
            AccountRows = await GetAccountRowsAsync();

            await AccountModal.HideAsync();
        }

        private void HandleViewSeriesClick(AccountRow item)
        {

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

        public class AccountRow
        {
            public int Id { get; set; }
            public string Number { get; set; } = default!;
            public string TypeName { get; set; } = default!;
            public string Nickname { get; set; } = default!;
            public int InstitutionId { get; set; }
            public string InstitutionName { get; set; } = default!;
            public HtmlString AccountNumberDetails { get; set; } = default!;
        }
    }
}