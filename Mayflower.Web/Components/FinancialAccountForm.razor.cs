using Mayflower.Core.DomainModels;
using Mayflower.Core.Extensions;
using Mayflower.Core.Infrastructure.Commands.FinancialAccounts;
using Mayflower.Web.Utilities;
using Microsoft.AspNetCore.Components;

namespace Mayflower.Web.Components
{
    public partial class FinancialAccountForm
    {
        [Parameter]
        public FinancialAccount? FinancialAccount { get; set; } = null;
        [Parameter]
        public EventCallback<int> OnCanceled { get; set; }
        [Parameter]
        public EventCallback<int> OnSaved { get; set; }

        private string _encryptedNumber = string.Empty;

        private int Id { get; set; }
        private string Number { get; set; } = default!;
        private string MaskedNumber { get; set; } = default!;
        private string TypeName { get; set; } = default!;
        private string Nickname { get; set; } = default!;
        private int InstitutionId { get; set; }
        private string InstitutionName { get; set; } = default!;
        private int AccountTypeId { get; set; }
        private bool ShowAccountNumberInput { get; set; }
        private IList<FormControlItem> AccountTypeOptions { get; set; } = new List<FormControlItem>();

        protected override async Task OnInitializedAsync()
        {
            AccountTypeOptions = GetAccountTypeOptions();
        }

        protected override void OnParametersSet()
        {
            SetFormData(FinancialAccount);
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

        private void SetFormData(FinancialAccount? account)
        {

            if (account == null)
            {
                ShowAccountNumberInput = true;
                return;
            }

            Id = account.Id;
            Nickname = account.Nickname;
            MaskedNumber = MaskAccountNumber(account.Number);
            AccountTypeId = (int)account.FinancialAccountTheme;
            AccountTypeOptions = UpdateSelectedItemInSelectList((int)account.FinancialAccountTheme, AccountTypeOptions);
            Number = account.Number;
        }

        private IList<FormControlItem> GetAccountTypeOptions()
        {
            var list = new List<FormControlItem>();

            foreach (var item in Enum.GetValues(typeof(FinancialAccountStyle)))
            {
                var type = (FinancialAccountStyle)item;

                if (type != FinancialAccountStyle.None)
                {
                    var description = type.ToDescription();

                    list.Add(new FormControlItem
                    {
                        Text = description,
                        Value = ((int)type).ToString()
                    });
                }
            }

            return list;
        }

        private IList<FormControlItem> UpdateSelectedItemInSelectList(int selectedId, IList<FormControlItem> list)
        {
            foreach (var item in list)
            {
                if (item.Value == selectedId.ToString())
                {
                    item.IsSelected = true;
                }
                else
                {
                    item.IsSelected = false;
                }
            }

            return list;
        }

        public async Task<bool> Save()
        {
            // TODO: add validation of some sort
            var command = new UpdateFinancialAccountCommand
            {
                Id = Id,
                Nickname = Nickname,
                Number = Number,
                FinancialInstitutionId = 1, //InstitutionId -- defaulting to 1 for now
                FinancialAccountTheme = (FinancialAccountStyle)AccountTypeId,
            };

            return await _commands.Execute(command);
        }

        private async Task HandleCancelClickAsync()
        {
            // Send Cancel event to parent
            await OnCanceled.InvokeAsync();
        }

        private async Task HandleSaveClickAsync()
        {
            var isSuccessful = await Save();
            // Send event to parent
            await OnSaved.InvokeAsync();
        }

        public class FormControlItem
        {
            private string _id = IdGenerator.GetNextId();

            public bool IsSelected { get; set; }
            public bool IsDisabled { get; set; }
            public string Text { get; set; } = default!;
            public string Value { get; set; } = default!;
            public string Id => _id;
        }
    }
}