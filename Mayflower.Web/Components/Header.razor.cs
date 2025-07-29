namespace Mayflower.Web.Components
{
    public partial class Header
    {
        protected override void OnInitialized()
        {
            _pageStateService.PageChanged += this.OnChange;
        }

        private void OnChange(object? sender, EventArgs e)
        {
            InvokeAsync(StateHasChanged);
        }

        public void Dispose()
        {
            _pageStateService.PageChanged -= this.OnChange;
        }
    }
}