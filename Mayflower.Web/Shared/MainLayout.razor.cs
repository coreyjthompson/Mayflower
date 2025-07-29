using Microsoft.AspNetCore.Components.Web;

namespace Mayflower.Web.Shared
{
    public partial class MainLayout
    {
        private const string MENU_DEFAULT_CSS_CLASS = "be-collapsible-sidebar-collapsed";
        private string? _menuStateCssClass = MENU_DEFAULT_CSS_CLASS;

        private void HandleMenuMouseOver(MouseEventArgs e)
        {
            _menuStateCssClass = string.Empty;
        }

        private void HandleMenuMouseOut(MouseEventArgs e)
        {
            _menuStateCssClass = MENU_DEFAULT_CSS_CLASS;
        }

    }
}