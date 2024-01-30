
namespace MauiBase.Events;

public class RootNavigationRequestedEventArgs : System.EventArgs
{
    public NavigationParameters Parameters { get; set; }
    public string PageName { get; set; }
    public BasePage Page { get; set; } = default!;
    public BasePopupPage PopupPage { get; set; } = default!;
    public bool IsAnimated { get; set; }
    public bool IsChildViewNavigation { get; set; }
    public bool IsBackNavigation { get; set; }
    public bool NavigateWithService { get; set; }
    public bool IgnoreStackInsertation { get; set; }
}
