using Mayflower.Web.Utilities;
using Microsoft.AspNetCore.Components;

namespace Mayflower.Web.Components.Core
{
    public abstract class MayflowerComponentBase : ComponentBase, IDisposable, IAsyncDisposable
    {
        #region Fields and Constants

        private bool _isAsyncDisposed;
        private bool _isDisposed;

        #endregion

        #region Methods

        /// <inheritdoc />
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            IsRenderComplete = true;

            await base.OnAfterRenderAsync(firstRender);
        }

        /// <inheritdoc />
        protected override void OnInitialized()
        {
            Id ??= IdGenerator.GetNextId();

            base.OnInitialized();
        }

        public static string BuildCssClasses(params (string? cssClass, bool when)[] cssClassList)
        {
            var list = new HashSet<string>();

            if (cssClassList is not null && cssClassList.Any())
            {
                foreach (var (cssClass, when) in cssClassList)
                {
                    if (!string.IsNullOrWhiteSpace(cssClass) && when)
                    {
                        list.Add(cssClass);
                    }
                }
            }

            if (list.Any())
            {
                return string.Join(" ", list);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string BuildCssClasses(string? userDefinedCssClass, params (string? cssClass, bool when)[] cssClassList)
        {
            var list = new HashSet<string>();

            if (cssClassList is not null && cssClassList.Any())
            {
                foreach (var (cssClass, when) in cssClassList)
                {
                    if (!string.IsNullOrWhiteSpace(cssClass) && when)
                    {
                        list.Add(cssClass);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(userDefinedCssClass))
            {
                list.Add(userDefinedCssClass.Trim());
            }

            if (list.Any())
            {
                return string.Join(" ", list);
            }
            else
            {
                return string.Empty;
            }
        }

        public static string BuildStyleTags(string? userDefinedCssStyle, params (string? cssStyle, bool when)[] cssStyleList)
        {
            var list = new HashSet<string>();

            if (cssStyleList is not null && cssStyleList.Any())
            {
                foreach (var (cssStyle, when) in cssStyleList)
                {
                    if (!string.IsNullOrWhiteSpace(cssStyle) && when)
                    {
                        list.Add(cssStyle);
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(userDefinedCssStyle))
            {
                list.Add(userDefinedCssStyle.Trim());
            }

            if (list.Any())
            {
                return string.Join(';', list);
            }
            else
            {
                return string.Empty;
            }
        }

        /// <inheritdoc />
        /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.idisposable?view=net-6.0" />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        /// <see href="https://learn.microsoft.com/en-us/dotnet/standard/garbage-collection/implementing-disposeasync#implement-both-dispose-and-async-dispose-patterns" />
        public async ValueTask DisposeAsync()
        {
            await DisposeAsyncCore(true).ConfigureAwait(false);

            Dispose(false);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    // cleanup
                }

                _isDisposed = true;
            }
        }

        protected virtual ValueTask DisposeAsyncCore(bool disposing)
        {
            if (!_isAsyncDisposed)
            {
                if (disposing)
                {
                    // cleanup
                }

                _isAsyncDisposed = true;
            }

            return ValueTask.CompletedTask;
        }

        #endregion

        #region Properties, Indexers

        [Parameter]
        public string? Class { get; set; }

        [Parameter]
        public string? Id { get; set; }

        [Parameter]
        public string? Name { get; set; }

        [Parameter]
        public string? Style { get; set; }

        protected virtual string? CssClasses => Class;

        protected ElementReference Element { get; set; }

        protected bool IsRenderComplete { get; private set; }

        protected virtual string? StyleTags => Style;

        #endregion

        #region Other

        ~MayflowerComponentBase()
        {
            Dispose(false);
        }

        #endregion
    }
}
