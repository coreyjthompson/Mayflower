namespace Mayflower.Web.Components
{
    public partial class Header
    {
        protected override void OnInitialized()
        {
            _changeNotificationService.HeaderChanged += this.OnChange;
        }

        private void OnChange(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _changeNotificationService.HeaderChanged -= this.OnChange;
        }
    }
}