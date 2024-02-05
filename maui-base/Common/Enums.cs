namespace MauiBase.Common;

public enum BottomAction
{
    More = 0,
    HomeView = 1,
    NewsView = 2,
    DirectoryView = 3,
    ActivityBoardView = 4,
    LeavesView = 5,
    NotificationsView = 6,
    PagesView = 7,
    SettingsView = 99
}

public enum ContentPopDirection
{
    LeftToRight,
    RightToLeft,
    TopToBottom,
    BottomToTop
}

public enum DisplayMode
{
    NoNavigationBar,
    NavigationBar
}

/// <summary>
/// Indicates about the navigation operation that has been invoked.
/// </summary>
public enum NavigationMode
{
    /// <summary>
    /// Indicates that a navigation operation occured that resulted in navigating backwards in the navigation stack.
    /// </summary>
    Back,
    /// <summary>
    /// Indicates that a new navigation operation has occured and a new page has been added to the navigation stack.
    /// </summary>
    New
}

public enum PageMode
{
    None,
    Menu,
    Navigate,
    Modal,
    ModalPopup
}
