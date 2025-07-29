using Microsoft.AspNetCore.Components;

namespace Mayflower.Web.Components
{
    public partial class TabGroup
    {
        [Parameter]
        public RenderFragment HeaderContent { get; set; } = default!;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;

        public TabPane ActiveTab { get; set; } = new TabPane();
        
        private List<TabPane> _tabs = new List<TabPane>();

        internal void AddTab(TabPane tab)
        {
            _tabs.Add(tab);

            if (_tabs.Count == 1)
            {
                ActiveTab = tab;
            }

            StateHasChanged();
        }

        string GetButtonCssClass(TabPane page)
        {
            var cssClass = "nav-link";

            if (page == ActiveTab)
            {
                cssClass += " active";
            }

            return cssClass;
        }

        void ActivateTab(TabPane tab)
        {
            ActiveTab = tab;
        }
    }
}