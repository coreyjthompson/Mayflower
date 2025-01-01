using Microsoft.AspNetCore.Components;

namespace Mayflower.Web.Components
{
    public partial class TabGroup
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public TabPane ActiveTab { get; set; }
        
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