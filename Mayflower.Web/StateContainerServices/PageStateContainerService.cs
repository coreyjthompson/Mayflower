using System.Reflection.PortableExecutable;

namespace Mayflower.Web.StateContainerServices
{
    public class PageStateContainerService
    {
        public string Title { get; set; } = string.Empty;

        public event EventHandler? PageChanged;

        public void SetPageProperties(string title)
        {
            Title = title;
            NotifyPageChanged();
        }

        public void NotifyPageChanged() => PageChanged?.Invoke(this, EventArgs.Empty);
    }
}
