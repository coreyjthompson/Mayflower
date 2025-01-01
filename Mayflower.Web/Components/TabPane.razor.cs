using Microsoft.AspNetCore.Components;

namespace Mayflower.Web.Components
{
    public partial class TabPane
    {
        [CascadingParameter]
        private TabGroup Parent { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public string Text { get; set; }

        protected override void OnInitialized()
        {
            if (Parent == null)
            {
                throw new ArgumentNullException(nameof(Parent), "TabPane must exist within a TabGroup");
            }

            base.OnInitialized();
            Parent.AddTab(this);
        }
    }
}