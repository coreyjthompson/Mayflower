using System.Reflection.PortableExecutable;

namespace Mayflower.Web.NotificationServices
{
    public class HeaderChangeNotificationService
    {
        public string Title { get; set; } = string.Empty;

        public event EventHandler? HeaderChanged;

        public void SetTitle(string title)
        {
            Title = title;
            HeaderChanged?.Invoke(this, EventArgs.Empty);
        }

        public void NotifyHeaderChanged() => HeaderChanged?.Invoke(this, EventArgs.Empty);
    }
}
