using Microsoft.AspNetCore.Components;

namespace Mayflower.Web.Components
{
    public partial class TabPane
    {
        [CascadingParameter]
        private TabGroup Parent { get; set; } = default!;

        [Parameter]
        public RenderFragment ChildContent { get; set; } = default!;
        [Parameter]
        public string Text { get; set; } = default!;

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