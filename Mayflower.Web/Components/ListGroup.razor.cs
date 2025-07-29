using System.Reflection.Metadata;
using Mayflower.Web.Components.Core;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Mayflower.Web.Components
{
    public partial class ListGroup<TItem> : MayflowerComponentBase
    {
        #region Properties, Indexers
        protected override string? CssClasses => BuildCssClasses(Class, ("list-group c-list-group", true));

        [Parameter]
        public RenderFragment<TItem>? ItemTemplate { get; set; }

        [Parameter]
        public IList<TItem> Data { get; set; } = default!;

        [Parameter]
        public RenderFragment EmptyDataTemplate { get; set; } = default!;

        [Parameter]
        public string EmptyText { get; set; } = "No records to display";

        private DotNetObjectReference<ListGroup<TItem>>? objRef;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override async ValueTask DisposeAsyncCore(bool disposing)
        {
            if (disposing)
            {
                Data = null!;
            }

            await base.DisposeAsyncCore(disposing);
        }

        protected override async Task OnInitializedAsync()
        {
            objRef ??= DotNetObjectReference.Create(this);

            await base.OnInitializedAsync();
        }

        protected override async Task OnParametersSetAsync()
        {
            StateHasChanged();
        }



        #endregion
    }
}